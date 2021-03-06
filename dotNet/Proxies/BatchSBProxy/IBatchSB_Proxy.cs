﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by wsdl, Version=1.1.4322.2032.
// 
namespace iLabs.Services {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    using iLabs.Architecture.DataTypes.ExperimentDataTypes;
    using iLabs.DataTypes.SoapHeaderTypes;
    using iLabs.DataTypes.TicketingDataTypes;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="IBatchSB", Namespace="http://ilab.mit.edu/iLabs/Services")]
    public class IBatchSB_Proxy : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        public sbAuthHeader sbAuthHeaderValue;
        
        /// <remarks/>
        public IBatchSB_Proxy() {
            this.Url = "http://localhost/ilabswsdl/IBatchSB.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/ListAllClientItems", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] ListAllClientItems() {
            object[] results = this.Invoke("ListAllClientItems", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginListAllClientItems(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ListAllClientItems", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string[] EndListAllClientItems(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/DeleteClientItem", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void DeleteClientItem(string name) {
            this.Invoke("DeleteClientItem", new object[] {
                        name});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginDeleteClientItem(string name, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("DeleteClientItem", new object[] {
                        name}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndDeleteClientItem(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/LoadClientItem", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LoadClientItem(string name) {
            object[] results = this.Invoke("LoadClientItem", new object[] {
                        name});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginLoadClientItem(string name, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("LoadClientItem", new object[] {
                        name}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndLoadClientItem(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/SaveClientItem", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SaveClientItem(string name, string itemValue) {
            this.Invoke("SaveClientItem", new object[] {
                        name,
                        itemValue});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSaveClientItem(string name, string itemValue, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SaveClientItem", new object[] {
                        name,
                        itemValue}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndSaveClientItem(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/Notify", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Notify(int experimentID) {
            this.Invoke("Notify", new object[] {
                        experimentID});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginNotify(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Notify", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndNotify(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/RetrieveResult", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ResultReport RetrieveResult(int experimentID) {
            object[] results = this.Invoke("RetrieveResult", new object[] {
                        experimentID});
            return ((ResultReport)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRetrieveResult(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveResult", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public ResultReport EndRetrieveResult(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ResultReport)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/Cancel", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool Cancel(int experimentID) {
            object[] results = this.Invoke("Cancel", new object[] {
                        experimentID});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginCancel(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Cancel", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndCancel(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/GetExperimentStatus", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public LabExperimentStatus GetExperimentStatus(int experimentID) {
            object[] results = this.Invoke("GetExperimentStatus", new object[] {
                        experimentID});
            return ((LabExperimentStatus)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetExperimentStatus(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetExperimentStatus", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public LabExperimentStatus EndGetExperimentStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((LabExperimentStatus)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/Submit", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public SubmissionReport Submit(string labServerID, string experimentSpecification, int priorityHint, bool emailNotification) {
            object[] results = this.Invoke("Submit", new object[] {
                        labServerID,
                        experimentSpecification,
                        priorityHint,
                        emailNotification});
            return ((SubmissionReport)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSubmit(string labServerID, string experimentSpecification, int priorityHint, bool emailNotification, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Submit", new object[] {
                        labServerID,
                        experimentSpecification,
                        priorityHint,
                        emailNotification}, callback, asyncState);
        }
        
        /// <remarks/>
        public SubmissionReport EndSubmit(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((SubmissionReport)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/Validate", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ValidationReport Validate(string labServerID, string experimentSpecification) {
            object[] results = this.Invoke("Validate", new object[] {
                        labServerID,
                        experimentSpecification});
            return ((ValidationReport)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginValidate(string labServerID, string experimentSpecification, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Validate", new object[] {
                        labServerID,
                        experimentSpecification}, callback, asyncState);
        }
        
        /// <remarks/>
        public ValidationReport EndValidate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ValidationReport)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/GetLabConfiguration", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetLabConfiguration(string labServerID) {
            object[] results = this.Invoke("GetLabConfiguration", new object[] {
                        labServerID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetLabConfiguration(string labServerID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetLabConfiguration", new object[] {
                        labServerID}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetLabConfiguration(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/GetLabInfo", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetLabInfo(string labServerID) {
            object[] results = this.Invoke("GetLabInfo", new object[] {
                        labServerID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetLabInfo(string labServerID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetLabInfo", new object[] {
                        labServerID}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetLabInfo(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/GetEffectiveQueueLength", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public WaitEstimate GetEffectiveQueueLength(string labServerID, int priorityHint) {
            object[] results = this.Invoke("GetEffectiveQueueLength", new object[] {
                        labServerID,
                        priorityHint});
            return ((WaitEstimate)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetEffectiveQueueLength(string labServerID, int priorityHint, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetEffectiveQueueLength", new object[] {
                        labServerID,
                        priorityHint}, callback, asyncState);
        }
        
        /// <remarks/>
        public WaitEstimate EndGetEffectiveQueueLength(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((WaitEstimate)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/GetLabStatus", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public LabStatus GetLabStatus(string labServerID) {
            object[] results = this.Invoke("GetLabStatus", new object[] {
                        labServerID});
            return ((LabStatus)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetLabStatus(string labServerID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetLabStatus", new object[] {
                        labServerID}, callback, asyncState);
        }
        
        /// <remarks/>
        public LabStatus EndGetLabStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((LabStatus)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/RetrieveSpecification", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RetrieveSpecification(int experimentID) {
            object[] results = this.Invoke("RetrieveSpecification", new object[] {
                        experimentID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRetrieveSpecification(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveSpecification", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndRetrieveSpecification(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/RetrieveLabConfiguration", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RetrieveLabConfiguration(int experimentID) {
            object[] results = this.Invoke("RetrieveLabConfiguration", new object[] {
                        experimentID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRetrieveLabConfiguration(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveLabConfiguration", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndRetrieveLabConfiguration(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/SaveAnnotation", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SaveAnnotation(int experimentID, string annotation) {
            object[] results = this.Invoke("SaveAnnotation", new object[] {
                        experimentID,
                        annotation});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSaveAnnotation(int experimentID, string annotation, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SaveAnnotation", new object[] {
                        experimentID,
                        annotation}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndSaveAnnotation(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/RetrieveAnnotation", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RetrieveAnnotation(int experimentID) {
            object[] results = this.Invoke("RetrieveAnnotation", new object[] {
                        experimentID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRetrieveAnnotation(int experimentID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RetrieveAnnotation", new object[] {
                        experimentID}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndRetrieveAnnotation(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("sbAuthHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ilab.mit.edu/iLabs/Services/GetExperimentInformation", RequestNamespace="http://ilab.mit.edu/iLabs/Services", ResponseNamespace="http://ilab.mit.edu/iLabs/Services", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ExperimentInformation[] GetExperimentInformation(int[] experimentIDs) {
            object[] results = this.Invoke("GetExperimentInformation", new object[] {
                        experimentIDs});
            return ((ExperimentInformation[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetExperimentInformation(int[] experimentIDs, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetExperimentInformation", new object[] {
                        experimentIDs}, callback, asyncState);
        }
        
        /// <remarks/>
        public ExperimentInformation[] EndGetExperimentInformation(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ExperimentInformation[])(results[0]));
        }
    }
 /*   
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://ilab.mit.edu/iLabs/type", IsNullable=false)]
    public class sbAuthHeader : System.Web.Services.Protocols.SoapHeader {
        
        /// <remarks/>
        public long coupon_ID;
        
        /// <remarks/>
        public string couponPassKey;
    }

	/// <remarks/>
	[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace="http://ilab.mit.edu/iLabs/type", IsNullable=false)]
	public class AuthHeader : System.Web.Services.Protocols.SoapHeader 
	{
        
		/// <remarks/>
		public string identifier;
        
		/// <remarks/>
		public string passkey;
	}
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class ExperimentInformation {
        
        /// <remarks/>
        public int experimentID;
        
        /// <remarks/>
        public int labServerID;
        
        /// <remarks/>
        public int userID;
        
        /// <remarks/>
        public int effectiveGroupID;
        
        /// <remarks/>
        public System.DateTime submissionTime;
        
        /// <remarks/>
        public System.DateTime completionTime;
        
        /// <remarks/>
        public System.DateTime expirationTime;
        
        /// <remarks/>
        public System.Double minTimeToLive;
        
        /// <remarks/>
        public int priorityHint;
        
        /// <remarks/>
        public int statusCode;
        
        /// <remarks/>
        public string[] validationWarningMessages;
        
        /// <remarks/>
        public string validationErrorMessage;
        
        /// <remarks/>
        public string[] executionWarningMessages;
        
        /// <remarks/>
        public string executionErrorMessage;
        
        /// <remarks/>
        public string annotation;
        
        /// <remarks/>
        public string xmlResultExtension;
        
        /// <remarks/>
        public string xmlBlobExtension;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class LabStatus {
        
        /// <remarks/>
        public bool online;
        
        /// <remarks/>
        public string labStatusMessage;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class ValidationReport {
        
        /// <remarks/>
        public bool accepted;
        
        /// <remarks/>
        public string[] warningMessages;
        
        /// <remarks/>
        public string errorMessage;
        
        /// <remarks/>
        public System.Double estRuntime;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class SubmissionReport {
        
        /// <remarks/>
        public ValidationReport vReport;
        
        /// <remarks/>
        public int labExperimentID;
        
        /// <remarks/>
        public System.Double minTimetoLive;
        
        /// <remarks/>
        public WaitEstimate wait;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class WaitEstimate {
        
        /// <remarks/>
        public int effectiveQueueLength;
        
        /// <remarks/>
        public System.Double estWait;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class ExperimentStatus {
        
        /// <remarks/>
        public int statusCode;
        
        /// <remarks/>
        public WaitEstimate wait;
        
        /// <remarks/>
        public System.Double estRuntime;
        
        /// <remarks/>
        public System.Double estRemainingRuntime;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class LabExperimentStatus {
        
        /// <remarks/>
        public ExperimentStatus statusReport;
        
        /// <remarks/>
        public System.Double minTimetoLive;
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ilab.mit.edu/iLabs/type")]
    public class ResultReport {
        
        /// <remarks/>
        public int statusCode;
        
        /// <remarks/>
        public string experimentResults;
        
        /// <remarks/>
        public string xmlResultExtension;
        
        /// <remarks/>
        public string xmlBlobExtension;
        
        /// <remarks/>
        public string[] warningMessages;
        
        /// <remarks/>
        public string errorMessage;
    }
	*/
}
