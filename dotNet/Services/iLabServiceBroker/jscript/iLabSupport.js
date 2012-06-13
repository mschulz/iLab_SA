/*
 * Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
 */

/* iLabSupport.js */

/*  This file will contain JavaScript used with the iLab ServiceBroker. THe first goal for this file is to provide
 support for creating iLab SCORM packages.

It depends on functions defined in SCORM_API_wrapper.js
SCORM wrapper v1.1.7 by Philip Hutchison, May 2008 (http://pipwerks.com).
Copyright (c) 2008 Philip Hutchison
*/

/* This is a first take and not fully defined. (Based on the test code Elio & PHB used )

    The SCO that would use this call will include the following:
	The SOAP Action - This may be the same for all iLab ServiceBrokers
	ClientAuthorization Coupon - used by the SB to retrieve a client authorization ticket
	LabClientGuid - the unique handle for the client
	GroupName - The serviceBroker group that is authorized to run the client
	ServiceBrokerWebServiceUrl
	

    The following will be retrieved from the LMS or SCORM player:
	USER_ID - This is the unique id for the user for that authority
	Possible other information

    The following will be extracted from the current request:
	Origin of the LMS
	Root of the WebSite
*/
   function launchLabRequest()
   {
      //var student=doLMSGetValue("cmi.core.student_id");	
	  //var cookies=document.cookie; 
      //alert(cookies);   
      var hostName = window.location.host;
      if( window.location.port.length >0){
        hostName += ":" + window.location.port;
      } 
      if( window.location.pathname.length > 2){
        if(window.location.pathname.indexOf("/",1) > 0){
            hostName += window.location.pathname.substr(0,window.location.pathname.indexOf("/",1));
        }
      }
     
	if (window.XMLHttpRequest)
  	{// code for IE7+, Firefox, Chrome, Opera, Safari
  		xmlhttp=new XMLHttpRequest();
 	 }
	else
  	{// code for IE6, IE5
  		xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
      }
      
  		// From SCORM
		var clientGuid = "TOD-12345";
		var groupname = "Experiment_Group";
		
		// From SCORM runtime-environment
		var userName = "elio";
		
		//Specific to the LMS
		var agentGuid = "UNED-1234567890-4321";
        var couponID = "16";
        var issuerGuid = "DC9F5D59-4A51-4793-8FC1-2BBE0510FC";
        var passkey ="12345-7777568";
        var duration = "-1";

      	var url= "http://ludi.mit.edu/ISB/ilabServiceBroker.asmx";
  		var soapXml = '<?xml version="1.0" encoding="utf-8"?>'+
				'<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" ' +
				'xmlns:xsd="http://www.w3.org/2001/XMLSchema" '+
				'xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">'+
  				'<soap12:Header>' +
    			//	'<AgentAuthHeader xmlns="http://ilab.mit.edu/iLabs/type">' +
    				  '<OperationAuthHeader xmlns="http://ilab.mit.edu/iLabs/type">' +
                        '<coupon  xmlns="http://ilab.mit.edu/iLabs/type">' +
                          '<couponId>' + couponID + '</couponId>' +
                          '<issuerGuid>' + issuerGuid + '</issuerGuid>' +
                          '<passkey>' + passkey +'</passkey>' +
                        '</coupon>'+
                     '</OperationAuthHeader>' +
			    //     '<agentGuid>' + agentGuid +'</agentGuid>' +
    			//	'</AgentAuthHeader>' +
  				'</soap12:Header>' +
  				'<soap12:Body>' +
    				'<LaunchLabClient xmlns="http://ilab.mit.edu/iLabs/Services">' +
      			        '<clientGuid>' + clientGuid +'</clientGuid>' +
      			        '<groupName>' + groupname + '</groupName>'+
      			        '<userName>' + userName +'</userName>' +
      			        '<authorityUrl>' + hostName + '</authorityUrl>' + 
      			        '<duration>' + duration + '</duration>' +
      			        '<autoStart>1</autoStart>'+
    				'</LaunchLabClient>' +
  				'</soap12:Body>'+
				'</soap12:Envelope>';
      	alert(soapXml);
      	xmlhttp.open("POST", url, false);
      	xmlhttp.setRequestHeader("Content-Type", "application/soap+xml; charset=utf-8");
      	xmlhttp.setRequestHeader("Content-Length", soapXml.length);
      	xmlhttp.send(soapXml); 

//      if (xmlhttp.readyState == 4) {
//	      //alert(xmlhttp.responseText);
		var objxml = xmlhttp.responseText; 
		alert(objxml);
		alert(xmlhttp.getAllResponseHeaders());
	
		//var value =objxml.getElementsByTagName("VALUE")[0].firstChild.nodeValue;
		 
//	} else {
//	  alert("no");
//      }
      
  

	}


 