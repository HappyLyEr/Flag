using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
    public class CrewInProject : BusinessClass
    {

        public CrewInProject()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GACrewInProject;
		}

        public static int GetAllCrewInProject(int CrewRowId)
        {
            return CrewInProjectDb.GetAllCrewInProject(CrewRowId);
        }
    }
}
