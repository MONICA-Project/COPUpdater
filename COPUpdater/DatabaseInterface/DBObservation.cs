using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using IO.Swagger.COPDBContext;

namespace IO.Swagger.DatabaseInterface
{
    public class DBObservation
    {
        public bool AddUpdateObservation(int? thingid, string streamid, DateTime? phenomentime, string Observationresult, ref string errorMessage, ref long newid)
        {
            bool retVal = true;

            using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
            {

                //Find type
                try
                {
                    var obs = context.LatestObservation
                           .Single(b => b.Thingid == thingid && b.Datastreamid == streamid);


                    obs.Thingid = (long)thingid;
                    obs.Datastreamid = streamid;
                    obs.Phenomentime = phenomentime;
                    //obs.Personid = personid;
                    //obs.Locationid = zoneid;
                    obs.Observationresult = Observationresult;

                    context.SaveChanges();

                }
                catch (Exception f)
                {
                    LatestObservation lo = new LatestObservation();
                    lo.Thingid = (long)thingid;
                    lo.Datastreamid = streamid;
                    lo.Phenomentime = phenomentime;
                    lo.Observationresult = Observationresult;
                    //lo.Personid = personid;
                    //lo.Locationid = zoneid;
                    context.LatestObservation.Add(lo);
                    context.SaveChanges();
                }
            }
            return retVal;
        }

        public bool ListObs(int thingId, ref string errorMessage)
        {
            bool retVal = true;
            try
            {
                using (COPDBContext.monica_cnetContext context = new COPDBContext.monica_cnetContext())
                {

                    //Make query 
                    var obs = (from d in context.LatestObservation  where d.Thingid == (long) thingId select d
                                     ).ToList();

                    if (obs == null || obs.Count() == 0)
                    {
                        errorMessage = "No things";
                        retVal = false;
                    }
                    else
                        foreach (var th in obs)
                        {
                            

                        }



                    //Insert role connection;
                }
                return retVal;
            }
            catch (Exception e)
            {
                errorMessage = "Database Excaption:" + e.Message + " " + e.StackTrace;
                return false;
            }

        }
    }
}

