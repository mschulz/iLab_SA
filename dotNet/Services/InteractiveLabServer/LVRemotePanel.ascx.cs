
namespace iLabs.LabServer.LabView
{
	using System;
    using System.Configuration;
	using System.Data;
	using System.Drawing;
    using System.Globalization;
    using System.Text;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

    using iLabs.UtilLib;

	/// <summary>
	///	Creates an OBJECT block which contains parameters for the LabVIEW RemotePanel runtime control.
    /// Currently this defaults to LabVIEW 2009, but the default may be changed by modifing the Web.config value 'LabViewVersion' 
    /// or by specifing the version in the Revision field on the labApplication page.
	/// </summary>
    public partial class LVRemotePanel : System.Web.UI.UserControl
    {
        public string version = null;
        public string viName = null;
        public string serverURL = null;
        public string hasControl = "true";
        public string scroll = "true";
        public int border = 1;
        public int width = 50;
        public int height = 50;

        protected string appMimeType = null;
        protected string codebase = null;
        protected string classId = null;
        protected string fpProtocol = null;
        protected string pluginspace = null;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if(ConfigurationManager.AppSettings["LabViewVersion"] != null && ConfigurationManager.AppSettings["LabViewVersion"].ToString().Length >0)
                version = ConfigurationManager.AppSettings["LabViewVersion"].ToString();
            if(Session["lvversion"] != null && Session["lvversion"].ToString().Length >0)
                version = Session["lvversion"].ToString();
            switch (version)
            {
                case "8.2":
                    appMimeType = @"application/x-labviewrpvi82";
                    classId = "CLSID:A40B0AD4-B50E-4E58-8A1D-8544233807AE";
                    fpProtocol = ".LV_FrontPanelProtocol.rpvi82";
                    pluginspace = @"http://digital.ni.com/express.nsf/express?openagent&code=ex3e33&";
                    // 8.2 uses different versions per language
                    StringBuilder buf = new StringBuilder();
                    CultureInfo culture = DateUtil.ParseCulture(Request.Headers["Accept-Language"]);

                    buf.Append(@"ftp://ftp.ni.com/support/labview/runtime/windows/8.2");
                    buf.Append(version);
                    switch (culture.ThreeLetterISOLanguageName)
                    {
                        case "fra":
                        case "fre":
                            buf.Append(@"/French");
                            break;
                        case "ger":
                        case "deu":
                            buf.Append(@"/German");
                            break;
                        case "jpn":
                            buf.Append(@"/Japanese");
                            break;
                        case "chi":
                        case "zho":
                            buf.Append(@"/Chinese");
                            break;
                        case "kor":
                            buf.Append(@"/Korean");
                            break;
                        default:
                            break;
                    }
                    buf.Append(@"/LVRunTimeEng.exe");
                    codebase = buf.ToString();
                    break;
                case "8.6":
                    appMimeType = "application/x-labviewrpvi86";
                    classId = "CLSID:A40B0AD4-B50E-4E58-8A1D-8544233807B0";
                    codebase = @"ftp://ftp.ni.com/support/labview/runtime/windows/8.6/LVRTE8.6min.exe";
                    fpProtocol = ".LV_FrontPanelProtocol.rpvi86";
                    pluginspace = @"http://digital.ni.com/express.nsf/bycode/exck2m";
                    break;
                case "2010":
                    appMimeType = "application/x-labviewrpvi100";
                    classId = "CLSID:A40B0AD4-B50E-4E58-8A1D-8544233807B2";
                    codebase = @"ftp://ftp.ni.com/support/labview/runtime/windows/2010/LVRTE2010min.exe";
                    fpProtocol = ".LV_FrontPanelProtocol.rpvi100";
                    pluginspace = @"http://digital.ni.com/express.nsf/bycode/exck2m";
                    break;
		        case "2011":
                    appMimeType = "application/x-labviewrpvi110";
                    classId = "CLSID:A40B0AD4-B50E-4E58-8A1D-8544233807B3";
                    codebase = @"ftp://ftp.ni.com/support/labview/runtime/windows/2011/LVRTE2011min.exe";
                    fpProtocol = ".LV_FrontPanelProtocol.rpvi110";
                    pluginspace = @"http://digital.ni.com/express.nsf/bycode/exck2m";
                    break;
                case "2009":
                default:
                    appMimeType = "application/x-labviewrpvi90";
                    classId = "CLSID:A40B0AD4-B50E-4E58-8A1D-8544233807B1";
                    codebase = @"ftp://ftp.ni.com/support/labview/runtime/windows/9.0/LVRTE90min.exe";
                    fpProtocol = ".LV_FrontPanelProtocol.rpvi90";
                    pluginspace = @"http://digital.ni.com/express.nsf/bycode/exck2m";
                    break;
            }
            // Put user code to initialize the page here


        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
