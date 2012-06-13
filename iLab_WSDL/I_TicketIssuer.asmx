<%@ WebService Language="c#" Class="I_TicketIssuer" %>

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml;

using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;

	/// <summary>
	/// Summary description for Ticketing Issuer.
	/// </summary>
	///
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
	[XmlType (Namespace="http://ilab.mit.edu/iLabs/type")]
	[WebServiceBinding(Name="ITicketIssuer",Namespace="http://ilab.mit.edu/iLabs/Services")]
	[WebService (Name="TicketIssuerProxy", Namespace="http://ilab.mit.edu/iLabs/Services",
		 Description="Ticketing  methods. Requests from the Issuers domain with coupons from another domain will be forwarded.")]

	public abstract class I_TicketIssuer : System.Web.Services.WebService
	{

        public AgentAuthHeader agentAuthHeader = new AgentAuthHeader();

		/// <summary>
		/// Attempts to add a ticket of the requested type
		/// to the existing coupon, fails if permissions 
		/// are not available.
		/// </summary>
		/// <param name="coupon">Ticket collection identifier, should be issued by this Ticketing service.</param>
        /// <param name="type">The ticket type</param>
        /// <param name="redeemerGuid">The final consumer of the ticket</param>
		/// <param name="duration">Long representation of the time the ticket will expire, this is not dependant on creation time.</param>
		/// <param name="payload"></param>
		/// <returns>the created Ticket or null if creation fails</returns>
		[WebMethod(Description="Attempts to add a ticket of the requested type to the existing coupon, fails if permissions are not available."), 
		SoapDocumentMethod(Binding="ITicketIssuer"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract bool AddTicket(Coupon coupon, string type, string redeemerGuid, 
			long duration, string payload); 
		

		/// <summary>
		/// Request the creation of a ticket of the specified type,
		/// by the Ticketing service. If the credentials pass a 
		/// ticket will be created and accessable by the returned coupon.
		/// </summary>
        /// <param name="type"></param>
        /// <param name="redeemerGuid">string GUID of the  requesting service</param>
        /// <param name="duration"> milliseconds from now, -1 for never expires</param>
		/// <param name="payload"></param>
		/// <returns>Coupon on success, null if Ticket creation is refused</returns>
		[WebMethod(Description="Request the creation of a ticket of the specified type, by the Ticketing service. If the credentials pass a ticket will be created and accessable by the returned coupon."), 
		SoapDocumentMethod(Binding="ITicketIssuer"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract Coupon CreateTicket(string type, string redeemerGuid,   
			long duration, string payload); 
		


		/// <summary>
		/// Redeem a ticket from this service, from the specifed coupon group.
		/// </summary>
		/// <param name="coupon"></param>
		/// <param name="redeemerGuid"></param>
		/// <param name="type"></param>
		/// <returns>the ticket or null</returns>
        [WebMethod(Description = "Redeem a ticket from this service, from the specifed coupon group."),
        SoapDocumentMethod(Binding = "ITicketIssuer"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract Ticket RedeemTicket(Coupon coupon, string type, string redeemerGuid);

		/// <summary>
		/// Request The cancellation of an individual ticket
		/// </summary>
		/// <param name="coupon"></param>
        /// <param name="type"></param>
        /// <param name="redeemerGuid"></param>
		/// <returns>True if the ticket has been cancelled successfully.</returns>
        [WebMethod(Description = "Request the cancellation of an individual ticket."),
        SoapDocumentMethod(Binding = "ITicketIssuer"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public abstract bool RequestTicketCancellation(Coupon coupon,
                string type, string redeemerGuid);






	}




