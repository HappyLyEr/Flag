using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
    public class LocationInCrew : BusinessClass
    {

        public LocationInCrew()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GALocationInCrew;
		}

        public static int GetAllLocationInCrew(int LocationRowId)
        {
            return LocationInCrewDb.GetAllLocationInCrew(LocationRowId);
        }
    }
}
