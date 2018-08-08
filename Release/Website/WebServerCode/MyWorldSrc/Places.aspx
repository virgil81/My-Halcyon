<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Places.aspx.vb" Inherits="Places" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="~/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SysMenu" Src="~/SysMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="~/Footer.ascx" %>

<!DOCTYPE html>
<html xmlns="https://www.w3.org/1999/xhtml" >
 <head>
  <title>Places - My World</title>
  <link href="/styles/Site.css" type="text/css" rel="stylesheet" />
  <link href="/Styles/TopMenu.css" type="text/css" rel="stylesheet" />
  <link href="/styles/TreeView.css" type="text/css" rel="stylesheet" />
  <link href="/styles/leaflet.css" type="text/css" rel="stylesheet" />
  <link href="/styles/map.css" type="text/css" rel="stylesheet" />
  <script type="text/javascript" src="/scripts/Cookie.js"></script>
  <script type="text/javascript" src="/scripts/TopMenu.js"></script>
  <script type="text/javascript" src="/scripts/TreeView.js"></script>
  <script type="text/javascript" src="/scripts/jquery-3.1.1.min.js"></script>
  <script type="text/javascript" src="/scripts/leaflet/leaflet.js"></script>
  <script type="text/javascript" src="/scripts/mustache.min.js"></script>
  <style type="text/css">
   /* Overrides for Site.css that apply only to the map page. */
   body { position: absolute; top: 0px; left: 0px; padding: 0; height: 100%; width: 100%; overflow: hidden; }
   #HeaderPos { position: absolute; top: 0; margin: 0; padding: 0; }
   #LSideBar { bottom: 62px;}
   #BodyArea, 
   #BodyAreaFull { position: absolute; top: 81px; left: 0; bottom: 62px; right: 0; box-sizing: border-box; }
   #FooterPos { position: absolute; bottom: 0; }
   #map-container { height: 100%; box-sizing: border-box; color: #3A3C3F; background: black; 
                    padding-top: 127px; /*transition: padding-left 0.2s;*/ }
  </style>
  <script type="text/javascript">
   /*
   L.CRS.scale = function(t) {
    return 512 * Math.Pow(2, t);
   }
   L.CRS.zoom = function(t) {
    return Math.log(t / 512) / Math.LN2
   }
   */

   $.namespace = function () {
    var o = null;
    var i, j, d;
    for (i = 0; i < arguments.length; i++) {
     d = arguments[i].split(".");
     o = window;
     for (j = 0; j < d.length; j++) {
      o[d[j]] = o[d[j]] || {};
      o = o[d[j]];
     }
    }
    return o;
   };

   $.namespace('$.my.maps.config');
   $.namespace('$.my.maps');
   $.my.maps.config = {
    my_base_url: "http://MyWorld.com/",
    tile_url: "http://MyWorld.com",
    default_title: "Welcome to My World",
    default_img: "/images/site/default-new.jpg",
    default_msg: "My World is a popular virtual world for education, entertainment and social interaction. <strong>If you have a virtual world viewer installed on your computer<strong>, sign up, teleport in and start exploring!",
    map_debug: false,
    map_grid_edge_size: 1048576,
    balloon_tmpl: ' \
        <div class="balloon-content"> \
            <h3> \
                {{#slurl}} \
                <a href="{{{slurl}}}" onclick="trakkit(\'maps\', \'teleport\', \'{{{slurl}}}\');"> \
                {{/slurl}} \
                    {{title}} \
                {{#slurl}} \
                </a> \
                {{/slurl}} \
            </h3> \
            {{#img}} \
            <a href="{{{slurl}}}" onclick="trakkit(\'maps\', \'teleport\', \'{{{slurl}}}\');"> \
                <img src="{{{img}}}" onError="this.onerror=null;this.src=assetsURL + \'/images/site/default-new.jpg\';" /> \
            </a> \
            {{/img}} \
            <p>{{{msg}}}</p> \
            <div class="buttons"> \
                {{#slurl}} \
                    <a class="HIGHLANDER_button_hot btn_large primary" title="visit this location" href="{{{slurl}}}" onclick="trakkit(\'maps\', \'teleport\', \'{{{slurl}}}\');">Visit this location</a> \
                {{/slurl}} \
                <a href="http://MyWorld.com/" target="_top" class="HIGHLANDER_button_hot btn_large secondary join_button">Join Now, it&rsquo;s free!</a> \
            </div> \
        </div>',
    noexists_tmpl: ' \
        <div id="map-error"> \
            <div id="error-content"> \
                <span class="error-close">Hide message</span> \
                <span class="location-title">We are unable to locate the region "{{{region_name}}}"</span> \
                <p>This region may no longer exist, but please double check your spelling and coordinates to make sure there aren&rsquo;t any errors and try again.</p> \
                <p>If your problem persists, contact <a href="http://MyWorld.com/">My World support</a></p> \
            </div> \
        </div>'
   }

   var myDebugMap = $.my.maps.config.map_debug;

   var MIN_ZOOM_LEVEL = 1;
   var MAX_ZOOM_LEVEL = 8;
   // Special class structure to provide a click process
   function myMap(mapElement, mapOptions) {
    var mapDiv = document.createElement("div");
    mapDiv.style.height = "100%";
    mapElement.appendChild(mapDiv);

    var tileOptions = {
     crs: L.CRS.Simple,
     minZoom: MIN_ZOOM_LEVEL,
     maxZoom: MAX_ZOOM_LEVEL,
     zoomOffset: 1,
     zoomReverse: true,
     bounds: [[0, 0], [$.my.maps.config.map_grid_edge_size, $.my.maps.config.map_grid_edge_size]],
     attribution: "<a href='" + $.my.maps.config.sl_base_url + "'>My World</a>",
     tileOptions: 256,
    };

    if (mapOptions.hasOwnProperty('tileSize') && is_number(mapOptions.tileSize)) {
     tileOptions.tileSize = mapOptions.tileSize;
    }

    var MyTileLayer = L.TileLayer.extend({
     getTileUrl: function (coords) {
      var data = {
       r: L.Browser.retina ? '@2x' : '',
       s: this._getSubdomain(coords),
       z: this._getZoomForUrl()
      };
      var regionsPerTileEdge = Math.pow(2, data['z'] - 1);
      data['region_x'] = coords.x * regionsPerTileEdge; // BUG: coords gets the wrong values when the tileSize is set to anything other than 256.  At 512 the values are half what they should be, but just scaling them up is incorrect as it causes tile skipping.
      data['region_y'] = (Math.abs(coords.y) - 1) * regionsPerTileEdge;
      return L.Util.template(this._url, L.extend(data, this.options));
     }
    });

    var tiles = new MyTileLayer("/map.aspx/map-{z}-{region_x}-{region_y}-objects.jpg", tileOptions);
    var map = L.map(mapElement, {
     crs: L.CRS.Simple,
     minZoom: MIN_ZOOM_LEVEL,
     maxZoom: MAX_ZOOM_LEVEL,
     maxBounds: [[0, 0], [$.my.maps.config.map_grid_edge_size, $.my.maps.config.map_grid_edge_size]],
     layers: [tiles]
    });
    map.on('click', function (event) {
     gotoSLURL(event.latlng.lng, event.latlng.lat, map);
    });
    return map;
   }

   // Call my page giving the region info clicked on, return in JSON. (multi-use processor)
   function gotoSLURL(x, y, lmap) {
    var int_x = Math.floor(x);
    var int_y = Math.floor(y);
    var local_x = Math.round((x - int_x) * 256);
    var local_y = Math.round((y - int_y) * 256);

    jQuery.get('/mapFunctions.aspx', { coordX: int_x, coordY: int_y, action: 'getRegionByCoordinates' }, function DisplayMapClick(jsonStr, status) {
     var regionData = JSON.parse(jsonStr);
     var debugInfo = '';
     if (myDebugMap) {
      debugInfo = ' x: ' + int_x + ' y: ' + int_y;
     }
     // Content may be like:
     // {"RegName":"Not Found","Error":"DB Error",}
     // {"RegName":"Delphi","Error":"REGION IS OFFLINE!"}
     // {"RegName":"Delphi","SLURL":"secondlife://Delphi/112/92/25"}
     // {"RegName":"Not Found","Error":"REGION NOT FOUND!"}
     //alert("Return: " + Return.toString());
     if (regionData.SLURL == null) {
      // Should use the slurl_setup() process to display?
      var content = '<div class="balloon-content balloon-content-narrow"><h3>' + regionData.RegName + '</h3>' + debugInfo + '<div></div></div>';
     }
     else {
      // Should use the slurl_setup() process to display?
      var content = '<div class="balloon-content balloon-content-narrow"><h3>' + regionData.RegName + '</h3>' + debugInfo + '<div class="buttons"><a href="' + regionData.SLURL + '" class="HIGHLANDER_button_hot btn_large primary">Visit this location</a></div></div>';
     }
     var popup = L.popup().setLatLng([y, x]).setContent(content).openOn(lmap);
    });
   }

   function slurlBuildValidator() {
    $('input#generate-slurl').click(function () {
     $('#build-location div.slurl-error').remove();
     var intX = parseFloat($('#x').val());
     var intY = parseFloat($('#y').val());
     var intZ = parseFloat($('#z').val());
     var imgRegExp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
     if ($('#windowImage').val()) {
      var imgCheck = $('#windowImage').val().search(imgRegExp);
     } else {
      var imgCheck = 0;
     }
     if (!$('#region').val() || (isNaN($('#x').val()) || isNaN(intX) || intX < 0 || intX > 256) || (isNaN($('#y').val()) || isNaN(intY) || intY < 0 || intY > 256) || (isNaN($('#z').val()) || isNaN(intZ)) || ($('#windowImage').val() != '' && imgCheck == -1)) {
      var error_content = "";
      if (!$('#region').val()) {
       error_content += "<p>A valid SLurl must contain a region name and x,y,z coordinates. Please make sure that each of these fields are properly filled in.</p>";
      }
      if (isNaN($('#x').val()) || isNaN($('#y').val()) || isNaN($('#z').val()) || isNaN(intX) || isNaN(intY) || isNaN(intZ) || intX > 256 || intY > 256) {
       error_content += "<p>The x and y coordinates must each contain numbers between 0 and 256 to be valid. z index must be between -99 and 999. All coordinates (x,y,z) must be numeric.</p>";
      }
      if (intX < 0 || intY < 0) {
       error_content += "<p>The x and y coordinates must be positive numbers.</p>";
      }
      if (imgCheck == -1) {
       error_content += "<p>Window Image must be a fully qualified url to a hosted image.";
       error_content += "( <strong>Example: </strong><em>http://www.yoursite.com/images/yourImage.jpg</em> )</p>";
      }
      $('#build-location legend').after('<div class="slurl-error"><h4>We\'re having trouble creating your SLurl</h4>' + error_content + '</div>');
      document.getElementById('slurl-builder').scrollTop = 0;
     } else {
      build_url();
     }
    })
   }

   function build_url() {
    var slurl = $('#slurl_base').val() + escape($('#region').val()) + "/" + parseFloat($('input#x').val()) + "/" + parseFloat($('input#y').val()) + "/" + parseFloat($('input#z').val()) + "/";
    $('#slurl-builder form #return-slurl').css({
     'display': 'block'
    });
    document.getElementById('slurl-builder').scrollTop = $('#slurl-builder').height() + $('#return-slurl').height();
    $('#output').val(slurl);
    $('#return-slurl').animate({
     backgroundColor: '#f8f8f8'
    }, 2500);
   }

   var slurl_data = {};
   var OptTitle = "<%=RTitle.ToString()%>";
   var OptImg = "<%=RImg.ToString()%>";
   var OptMsg = "<%=RMsg.ToString()%>";

   function slurl_setup() {
    var urlPath = window.location.pathname;
    var urlParts = urlPath.split("/");
    var initial_region = 'Welcome';
    slurl_data['region'] = new Object();
    if (urlParts[1] == '' || urlParts[1] == 'Places.aspx' || urlParts[3] == undefined) {
     slurl_data['region']['name'] = initial_region;
     slurl_data['region']['x'] = 0;
     slurl_data['region']['y'] = 0;
     slurl_data['region']['z'] = 0;
     slurl_data['region']['default'] = true;
    }
    else {
     initial_region = (urlParts[2] == undefined) ? 'Welcome' : decodeURIComponent(urlParts[2]);
     slurl_data['region']['name'] = initial_region;
     slurl_data['region']['x'] = (urlParts[3] != undefined) ? check_coords(urlParts[3]) : 128;
     slurl_data['region']['y'] = (urlParts[4] != undefined) ? check_coords(urlParts[4]) : 128;
     slurl_data['region']['z'] = (urlParts[5] != undefined) ? urlParts[5] : 0;
     slurl_data['region']['default'] = false;
    }
    var slurl = 'secondlife://' + encodeURIComponent(slurl_data['region']['name'].toUpperCase()) + '/' + slurl_data['region']['x'] + '/' + slurl_data['region']['y'] + '/' + slurl_data['region']['z'];
    var SetTitle = $.my.maps.config.default_title; // Set to default contents
    var SetImg = $.my.maps.config.default_img;
    var SetMsg = $.my.maps.config.default_msg;
    if (OptTitle != "") {                            // Check for optional supplied content
     SetTitle = OptTitle;
    }
    if (OptImg != "") {
     SetImg = OptImg;
    }
    if (OptMsg != "") {
     SetMsg = OptMsg;
    }
    var templateData = {                           // Send content to display
     'slurl': slurl,
     'title': SetTitle,
     'img': SetImg,
     'msg': SetMsg
    };
    // http://maps-lecs.lindenlab.com/agni/32/_img/default-new.jpg image size info
    Mustache.parse($.my.maps.config.balloon_tmpl);
    slurl_data['windowcontent'] = Mustache.render($.my.maps.config.balloon_tmpl, templateData);
    slurl_data['sidebarcontent'] = templateData;
    Mustache.parse($.my.maps.config.noexists_tmpl);
    slurl_data['noexist_windowcontent'] = Mustache.render($.my.maps.config.noexists_tmpl, {
     'region_name': slurl_data['region']['name']
    });
   }

   function loadmap(firstJoinUrl, zoomLevel, coordScalar/*HACK*/) {
    if (typeof (zoomLevel) === "undefined") {
     zoomLevel = 6;
    }
    var latlng = L.latLng(/*Y=*/1000.5 * coordScalar/*HACK*/, /*X=*/1000.5 * coordScalar/*HACK*/);
    var $body = $('body');
    if ($body.data('region-coords-error')) { // The user provided bad coordinates, as indicated by serverside via the body tag's attribute data-region-coords-error
     var errorContent = slurl_data['noexist_windowcontent'];
     $('#marker0').remove();
     /*
     $('#location-heading').html('Destination Guide Picks');
     $('#dest-guide-title').remove();
     $('body').prepend(errorContent);
     $('#map-error #error-content .error-close').click(function () {
      $('#map-error').hide();
     });*/
     $mymap.setView(latlng, zoomLevel);
    }
    else if (!slurl_data['region']['default']) { // The user provided a coordinate in the URL.
     // Convert from region-coordinates to map coordinates using the data-region-coords-x and data-region-coords-y attibutes of the body tag, provided by serverside for this selected region.
     var x = parseInt($body.data('region-coords-x')) + slurl_data['region']['x'] / 256;
     var y = parseInt($body.data('region-coords-y')) + slurl_data['region']['y'] / 256;
     latlng = L.latLng(y * coordScalar, x * coordScalar);
     var bubbleContent = slurl_data['windowcontent'].replace('/\s+/', ' ');
     var $content = $('<div/>').html(bubbleContent);
     if (firstJoinUrl) {
      $content.find('a.join_button').attr('href', firstJoinUrl);
     }
     /*var marker = makeMarkerForSidebar($content.get(0), latlng, $('#marker0'));*/
     $mymap.setView(latlng, MAX_ZOOM_LEVEL);
     /*marker.openPopup();*/
    }
    else { // Default. Body tag would have data-region-coords-default="true" but doesn't need it.
     $mymap.setView(latlng, zoomLevel);
    }
   }

   function makeMarkerForSidebar(windowContent, markerLatLng, $sidebarItem, regionLocation) {
    var domId = $sidebarItem.attr('id');
    var marker = L.marker(markerLatLng, {
     icon: L.icon({
      iconUrl: "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0nMjAwJyBoZWlnaHQ9JzIwMCcgZmlsbD0iIzAwMDAwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDEwMCAxMDAiIGVuYWJsZS1iYWNrZ3JvdW5kPSJuZXcgMCAwIDEwMCAxMDAiIHhtbDpzcGFjZT0icHJlc2VydmUiPjxnIGRpc3BsYXk9Im5vbmUiPjxwYXRoIGRpc3BsYXk9ImlubGluZSIgZD0iTTg0LjkzOCwyNi4wNDRIMTUuMDYyYy0wLjU0NywwLTAuOTk4LDAuNDQ2LTAuOTk4LDAuOTk3djQ1LjkxNmMwLDAuNTUzLDAuNDUxLDAuOTk5LDAuOTk4LDAuOTk5aDY5Ljg3NSAgIGMwLjU1NSwwLDAuOTk4LTAuNDQ2LDAuOTk4LTAuOTk5VjI3LjA0MUM4NS45MzYsMjYuNDksODUuNDkyLDI2LjA0NCw4NC45MzgsMjYuMDQ0eiBNODMuNzc3LDQ2LjU4OUgxNi4yM3YtOS45OGg2Ny41NDdWNDYuNTg5eiIvPjwvZz48ZyBkaXNwbGF5PSJub25lIj48cGF0aCBkaXNwbGF5PSJpbmxpbmUiIGQ9Ik04NS45OCwzNy4xNDRjMC0xMC40NzYtOC4yNTgtMTguNzE5LTE4LjQzNC0xOC43MTljLTguMjE3LDAtMTUuMTY4LDUuNjYxLTE3LjU0OSwxMy4zMSAgIGMtMi4zODMtNy42NDgtOS4zMzQtMTMuMTEzLTE3LjU1MS0xMy4xMTNjLTEwLjE3OCwwLTE4LjQyOCw4LjAwOC0xOC40MjgsMTguNDg4YzAsNS4yODgsMi4xMDcsOS4wMjQsNS40OTgsMTMuMDIyaC0wLjA0OSAgIGwzMC41NDcsMzEuNDQybDMwLjU0My0zMS40NDJoLTAuMDhDODMuODcxLDQ2LjEzNCw4NS45OCw0Mi40MjgsODUuOTgsMzcuMTQ0eiIvPjwvZz48Zz48cGF0aCBkPSJNNTAsMTQuMDY0Yy0xMy4yMywwLTIzLjk1NywxMC43MjYtMjMuOTU3LDIzLjk1N0MyNi4wNDMsNTEuMjUsNTAsODUuOTM2LDUwLDg1LjkzNlM3My45NTcsNTEuMjUsNzMuOTU3LDM4LjAyMSAgIEM3My45NTcsMjQuNzksNjMuMjI5LDE0LjA2NCw1MCwxNC4wNjR6IE01MCw1MC40OTdjLTYuNjE3LDAtMTEuOTc5LTUuMzU5LTExLjk3OS0xMS45NzdjMC02LjYxNiw1LjM2MS0xMS45NzgsMTEuOTc5LTExLjk3OCAgIGM2LjYxMywwLDExLjk3OSw1LjM2MiwxMS45NzksMTEuOTc4QzYxLjk3OSw0NS4xMzgsNTYuNjEzLDUwLjQ5Nyw1MCw1MC40OTd6Ii8+PC9nPjxnIGRpc3BsYXk9Im5vbmUiPjxwb2x5Z29uIGRpc3BsYXk9ImlubGluZSIgcG9pbnRzPSI4NS45ODIsMzIuMDM3IDg1Ljk4MiwxNC4wNTEgODUuOTgyLDE0LjA0NyA2Ny45OTMsMTQuMDQ3IDY3Ljk5MywxNC4wNDcgMjYuMDEzLDE0LjA0NyAgICAyNi4wMTMsMzIuMDM3IDUwLjk3MSwzMi4wMzcgMTQuMDE4LDY4Ljk5IDMwLjk4LDg1Ljk1MyA2Ny45OTMsNDguOTQyIDY3Ljk5Myw3NC4wMTcgODUuOTgyLDc0LjAxNyA4NS45ODIsMzIuMDM3ICAiLz48L2c+PGcgZGlzcGxheT0ibm9uZSI+PHBvbHlnb24gZGlzcGxheT0iaW5saW5lIiBwb2ludHM9IjYyLjAxMSwxNC4xMDcgMzguMDIyLDE0LjEwNyA1NC4wMTcsMzguMDk3IDE0LjAzNCwzOC4wOTcgMTQuMDM0LDYyLjA4MiA1NC4wMTUsNjIuMDgyICAgIDM4LjAyMiw4Ni4wNzEgNjIuMDExLDg2LjA3MSA4Niw1MC4wODcgICIvPjwvZz48L3N2Zz4=",
      iconSize: [53, 48],
      iconAnchor: [26, 48]
     })
    }).addTo($mymap);
    marker.bindPopup(windowContent, {
     minWidth: 350
    }).on('popupopen', function (event) {
     $('#' + domId).addClass('result-selected');
     if (!regionLocation) {
      return;
     }
     var $joinButton = $(event.popup.getPane()).find('a.join_button');
     if (0 < $joinButton.size()) {
      directSlurl($joinButton, regionLocation);
     }
    }).on('popupclose', function (event) {
     $('#' + domId).removeClass('result-selected');
    });
    $sidebarItem.click(function (evt) {
     evt.preventDefault();
     marker.togglePopup();
    });
    return marker;
   }

   function check_coords(slurl_coord) {
    if (is_number(slurl_coord) && ((0 <= slurl_coord) && (slurl_coord <= 256))) {
     return slurl_coord;
    } else {
     return 128;
    }
   }

   function is_number(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
   }

   slurl_setup();

   // *** Page functions ***
   var $mymap = {};
   $(function () {
    //var headerHeight = jQuery("#HeaderPos").outerHeight(true);
    //var footerHeight = jQuery("#FooterPos").outerHeight(true);
    //var mapDiv = document.getElementById("BodyArea");
    //mapDiv.style.paddingBottom = footerHeight + "px";
    //mapDiv.style.paddingTop = headerHeight + "px";

    // Get the tile size from the ocean tile.
    var oceanImage = new Image();
    oceanImage.onload = function () {
     var zoomLookup = {
      128: 7,
      256: 6,
      512: 5,
      1024: 4,
     }

     var options = {};

     if (this.width > 0) {
      options.tileSize = this.width;
     }

     $mymap = myMap(document.getElementById('map-container'), options);
     var firstJoinUrl = null; //'http://3dworld/Register.aspx'; // To be supplied by serverside if wanted.  Or hardcode, but meh.

     var zoomLevel = 6;
     if (zoomLookup.hasOwnProperty(options.tileSize)) {
      zoomLevel = zoomLookup[options.tileSize];
     }

     loadmap(firstJoinUrl, zoomLevel, options.tileSize / 256/*HACK*/);
    };
    oceanImage.src = "/map.aspx/map-a-a-a-objects.jpg";
   });
   // References:
   // http://leafletjs.com/reference-1.1.0.html and http://leafletjs.com/examples/quick-start/
   // Compare with http://places.inworldz.com/ and http://maps.secondlife.com/secondlife/DayStar/153/112/25/?title=SDA%2520Church&img=http://www.bibleprophecyisland.com/Images/Church/Links/ChurchFrontMap.jpg&msg=Worship%2520with%2520us%2520in%2520our%2520church%2520service
  </script>
 </head>
 <body runat="server" id="Body">
  <!-- Built from MyWorld Basic Page template v. 1.0 -->
  <div id="BodyArea">
   <div id="map-container"></div>
  </div>
  <div id="LSideBar">
   <table class="SidebarCtl">
    <tr>
     <td class="ProgTitle">My World Places</td>
    </tr>
    <tr>
     <td class="SidebarSpacer">&nbsp;</td>
    </tr>
    <!-- Sidebar Menu Control -->
    <tr>
     <td id="SidebarMenu" runat="server">
      <!-- Sidebar menu content is set in code -->
     </td>
    </tr>
   </table>
  </div>
  <div id="HeaderPos">
   <uc1:Header id="Header1" runat="server"></uc1:Header>
   <uc1:SysMenu id="SysMenu1" runat="server"></uc1:SysMenu>
  </div>
  <div id="FooterPos">
   <uc1:Footer id="Footer" runat="server"></uc1:Footer>
  </div>
 </body>
</html>
