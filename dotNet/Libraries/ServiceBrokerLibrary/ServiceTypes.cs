using System;

namespace iLabs.Architecture
{
	

	public class ServiceTypes
	{ 
		public enum ServiceType : int
		{
			SERVICE_BROKER = 1, LAB_SERVER=2, EXPERIMENT_STORAGE_SERVER=4, 
			SCHEDULING_SERVER=8, LAB_LAB_SCHEDULING_SERVER=9 }

		// Should these be replaced with an enum

		
		public const string SERVICE_BROKER = "SERVICE BROKER";
		public const string LAB_SERVER = "LAB SERVER";
		public const string EXPERIMENT_STORAGE_SERVER = "EXPERIMENT STORAGE SERVER";
		public const string SCHEDULING_SERVER =	"SCHEDULING SERVER";
		public const string LAB_SCHEDULING_SERVER =	"LAB SCHEDULING SERVER";
	}  
}