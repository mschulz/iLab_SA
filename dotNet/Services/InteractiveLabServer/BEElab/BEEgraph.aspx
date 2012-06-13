<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.BEEgraph" CodeFile="BEEgraph.aspx.cs"  EnableSessionState="true"%>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="../labNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>

<!DOCTYPE html >
<html lang="en">
	<head>
		<title>Building Energy Efficiency Lab</title> 
		<!-- 
Copyright (c) 2012 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
        <meta name="description" content="BEE Lab Review" />
        <meta name="keywords" content="ilab, MIT, CECI, BEE Lab" />
        <meta name="author" content="CECI" />
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<style type="text/css">@import url( ../css/main.css );
		</style>
		
        <!--[if lt IE 9]>
            <script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
            <![endif]-->
		<script type="text/javascript" src="http://code.jquery.com/jquery-1.7.2.min.js"></script>
		<script type="text/javascript" src="http://cdn.jquerytools.org/1.2.7/all/jquery.tools.min.js"></script>
        <script type="text/javascript" src="scripts/vendor/underscore.js"></script>
        <script type="text/javascript" src="scripts/vendor/highstock.js"></script>
        <script type="text/javascript" src="scripts/vendor/exporting.src.js"></script>
        <script type="text/javascript" src="scripts/application.js"></script>
        <link href='http://fonts.googleapis.com/css?family=Imprima' rel='stylesheet' type='text/css'/>
        <link href='styles/application.css' rel='stylesheet' type='text/css'/>
	</head>
	<body>
		<form method="post" runat="server">
		    <asp:HiddenField id="hdnExperimentID" runat="server" />
			<asp:HiddenField id="hdnMin" runat="server" />
			<asp:HiddenField id="hdnMax" runat="server" />
			<asp:HiddenField id="hdnSensors" runat="server" />
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server" BannerText="Building Energy Lab"></uc1:banner>
			<uc1:userNav id="UserNav1" runat="server" ></uc1:userNav>
			<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1><asp:label id="lblExperimentTitle" Runat="server"><% =title %></asp:label></h1>
						<h1>Welcome back! Here's your data</h1>
						<asp:label id="lblDescription" Runat="server"></asp:label>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
					    <p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>
				

    <input type="checkbox" class="series-box north-interior" id="cb-north-interior" value="58" name="north-interior"/>
    <label for="cb-north-interior">north-interior</label>

    <input type="checkbox" class="series-box north-exterior" id="cb-north-exterior" value="15" name="north-exterior"/>
    <label for="cb-north-exterior">north-exterior</label>

    <input type="checkbox" class="series-box south-interior" id="cb-south-interior" value="34" name="south-interior"/>
    <label for="cb-south-interior">south-interior</label>

    <input type="checkbox" class="series-box south-exterior" id="cb-south-exterior" value="9" name="south-exterior"/>
    <label for="cb-south-exterior">south-exterior</label>

    <input type="checkbox" class="series-box east-interior" id="cb-east-interior" value="30" name="east-interior"/>
    <label for="cb-east-interior">east-interior</label>

    <input type="checkbox" class="series-box east-exterior" id="cb-east-exterior" value="6" name="east-exterior"/>
    <label for="cb-east-exterior">east-exterior</label>

    <input type="checkbox" class="series-box west-interior" id="cb-west-interior" value="54" name="west-interior"/>
    <label for="cb-west-interior">west-interior</label>

    <input type="checkbox" class="series-box west-exterior" id="cb-west-exterior" value="11" name="west-exterior"/>
    <label for="cb-west-exterior">west-exterior</label>

    <input type="checkbox" class="series-box floor-interior" id="cb-floor-interior" value="64" name="floor-interior"/>
    <label for="cb-floor-interior">floor-interior</label>

    <input type="checkbox" class="series-box floor-exterior" id="cb-floor-exterior" value="22" name="floor-exterior"/>
    <label for="cb-floor-exterior">floor-exterior</label>

    <input type="checkbox" class="series-box ceil-interior" id="cb-ceil-interior" value="60" name="ceil-interior"/>
    <label for="cb-ceil-interior">ceil-interior</label>

    <input type="checkbox" class="series-box ceil-exterior" id="cb-ceil-exterior" value="19" name="ceil-exterior"/>
    <label for="cb-ceil-exterior">ceil-exterior</label>

    <input type="checkbox" class="series-box air-north" id="cb-air-north" value="66" name="air-north"/>
    <label for="cb-air-north">air-north</label>

    <input type="checkbox" class="series-box air-south" id="cb-air-south" value="42" name="air-south"/>
    <label for="cb-air-south">air-south</label>
  <div class="select-series">
    <a href='#' rel='#bee-modal'>Select Graphed Sensors</a>
  </div>
  <asp:Button ID="btnDownload" runat="server" OnClick="downloadClick"  Text="Download"/>
  <div id="container">Please wait while we load it. This may take a while.</div>

  <div class="modal" id="bee-modal">
    <div id="bee-room-modal">

      <div class="sensor north interior" data-checkbox="cb-north-interior" title="interior sensor @ north wall">
        iN
      </div>

      <div class="sensor north exterior" data-checkbox="cb-north-exterior" title="exterior sensor @ north wall">
        eN
      </div>

      <div class="sensor south interior" data-checkbox="cb-south-interior" title="interior sensor @ south wall">
        iS
      </div>

      <div class="sensor south exterior" data-checkbox="cb-south-exterior" title="exterior sensor @ south wall">
        eS
      </div>

      <div class="sensor east interior" data-checkbox="cb-east-interior" title="interior sensor @ east wall">
        iE
      </div>

      <div class="sensor east exterior" data-checkbox="cb-east-exterior" title="exterior sensor @ east wall">
        eE
      </div>

      <div class="sensor west interior" data-checkbox="cb-west-interior" title="interior sensor @ west wall">
        iW
      </div>

      <div class="sensor west exterior" data-checkbox="cb-west-exterior" title="exterior sensor @ west wall">
        eW
      </div>

      <div class="sensor floor interior" data-checkbox="cb-floor-interior" title="interior sensor @ floor wall">
        iF
      </div>

      <div class="sensor floor exterior" data-checkbox="cb-floor-exterior" title="exterior sensor @ floor wall">
        eF
      </div>

      <div class="sensor ceil interior" data-checkbox="cb-ceil-interior" title="interior sensor @ ceil wall">
        iC
      </div>

      <div class="sensor ceil exterior" data-checkbox="cb-ceil-exterior" title="exterior sensor @ ceil wall">
        eC
      </div>

      <div class="sensor air north" data-checkbox="cb-air-north" title="air sensor near north wall">
        aN
      </div>

      <div class="sensor air south" data-checkbox="cb-air-south" title="air sensor near south wall">
        aS
      </div>
    </div> <!-- End of sensors -->
    <div class="legend"></div>
  </div>

					</div><!-- end pagecontent div -->
					<br clear="all" />
				</div><!-- end innerwrapper div -->
				<div><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
			</div>
		</form>
	</body>
</html>
