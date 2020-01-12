using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.COPDBContext;

namespace IO.Swagger.DatabaseInterface
{
    public class DBIncident
    {

        public bool AddIncident(string description, string itype, string position, int prio, string status, double probability, string iplan, DateTime itime, string wid, string phoneno, string AdditionalMedia, string MediaType, string Area, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    Incident a = new Incident();
                    a.Description = description;
                    a.Incidenttime = itime;
                    a.Interventionplan = iplan;
                    a.Position = position;
                    a.Prio = prio;
                    a.Probability = probability;
                    a.Status = status;
                    a.Type = itype;
                    a.WearablePhysicalId = wid;
                    a.PhoneNumber = phoneno;
                    a.AdditionalMedia = AdditionalMedia;
                    a.AdditionalMediaType = MediaType;
                    a.Area = Area;
                    context.Incident.Add(a);
                    context.SaveChanges();

                    newid = a.Incidentid;

                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Exception:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }

        public bool UpdateIncident(int id, string description, string itype, string position, int prio, string status, double probability, string iplan, DateTime itime, string wid, string phoneno, string AdditionalMedia, string MediaType, string Area, ref string errorMessage, ref long newid)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {


                    var a = context.Incident.FirstOrDefault(Incident => Incident.Incidentid == id);
                   if(a != null)
                    {
                        a.Description = description;
                        a.Incidenttime = itime;
                        a.Interventionplan = iplan;
                        a.Position = position;
                        a.Prio = prio;
                        a.Probability = probability;
                        a.Status = status;
                        a.Type = itype;
                        a.WearablePhysicalId = wid;
                        a.PhoneNumber = phoneno;
                        a.AdditionalMedia = AdditionalMedia;
                        a.AdditionalMediaType = MediaType;
                        a.Area = Area;
                        context.Incident.Update(a);
                        context.SaveChanges();
                    }
                   else
                    {
                        errorMessage = "Incident with ID :" + id + "  does not exist";
                        return false;
                    }
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Exception:" + e.Message + " " + e.StackTrace;
                return false;
            }
        }

     
    }
}
