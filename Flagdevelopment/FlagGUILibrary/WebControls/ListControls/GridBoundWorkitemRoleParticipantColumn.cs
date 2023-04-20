using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Data;
using GASystem.BusinessLayer;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundWorkitemRoleParticipantColumn : GridBoundColumn
    {
        private string _separator = "<br/>";
        const int MAXPARTICIPANTSINLIST = 5;

        public string Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        public override bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        protected override string FormatDataValue(object dataValue, GridItem item)
        {

            if (dataValue == null)
                return string.Empty;

            string val = base.FormatDataValue(dataValue, item);


            string participantIds = string.Empty;
            if (val.Length > 2)
            {
                participantIds = val.Substring(1, val.Length - 2);
                participantIds = participantIds.Replace(";", ",");
            }
            else return string.Empty;



            string[] participantRoles = participantIds.Split(',');

            string resp = string.Empty;
            if (participantRoles.Length > 0)
            {
                if (participantRoles.Length == 1)
                {
                    try
                    {
                        resp = Lists.GetListDescriptionByRowId(int.Parse(participantRoles[0]));
                    }
                    catch (GASystem.GAExceptions.GADataAccessException gaex)
                    {
                        //TODO LOG
                    }
                    catch (System.ArgumentException aex)
                    {
                        //todo log
                    }
                    catch (System.FormatException fex)
                    {
                        //todo log
                    }
                }
                else
                {
                    for (int t = 0; t < participantRoles.Length && t < MAXPARTICIPANTSINLIST; t++)
                    {
                        try
                        {
                            resp += Lists.GetListDescriptionByRowId(int.Parse(participantRoles[t])) + _separator;
                        }
                        catch (GASystem.GAExceptions.GADataAccessException gaex)
                        {
                            //TODO LOG
                        }
                        catch (System.ArgumentException aex)
                        {
                            //todo log
                        }
                        catch (System.FormatException fex)
                        {
                            //todo log
                        }
                    }
                }
                if (participantRoles.Length > MAXPARTICIPANTSINLIST)
                    resp += "...";
                }
            return resp;
        } 
    }
}
