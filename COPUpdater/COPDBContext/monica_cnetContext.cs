using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IO.Swagger.COPDBContext
{
    public partial class monica_cnetContext : DbContext
    {
        public monica_cnetContext()
        {
        }

        public monica_cnetContext(DbContextOptions<monica_cnetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<AdminLayer> AdminLayer { get; set; }
        public virtual DbSet<AdminLayerDets> AdminLayerDets { get; set; }
        public virtual DbSet<AdminMenu> AdminMenu { get; set; }
        public virtual DbSet<AdminMenuRoles> AdminMenuRoles { get; set; }
        public virtual DbSet<Datastream> Datastream { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventLocations> EventLocations { get; set; }
        public virtual DbSet<EventOrganizations> EventOrganizations { get; set; }
        public virtual DbSet<EventServicesProperties> EventServicesProperties { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<FeatureOfInterest> FeatureOfInterest { get; set; }
        public virtual DbSet<Fffriends> Fffriends { get; set; }
        public virtual DbSet<FriendfinderReg> FriendfinderReg { get; set; }
        public virtual DbSet<Incident> Incident { get; set; }
        public virtual DbSet<InterventionActions> InterventionActions { get; set; }
        public virtual DbSet<InterventionPlan> InterventionPlan { get; set; }
        public virtual DbSet<LatestObservation> LatestObservation { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LocationRestrictions> LocationRestrictions { get; set; }
        public virtual DbSet<LocationServices> LocationServices { get; set; }
        public virtual DbSet<LocationTemplates> LocationTemplates { get; set; }
        public virtual DbSet<LocationThings> LocationThings { get; set; }
        public virtual DbSet<Observation> Observation { get; set; }
        public virtual DbSet<ObservedProperty> ObservedProperty { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonRoles> PersonRoles { get; set; }
        public virtual DbSet<PersonThings> PersonThings { get; set; }
        public virtual DbSet<ProAcousticFeedback> ProAcousticFeedback { get; set; }
        public virtual DbSet<ProAcousticFeedbackTypes> ProAcousticFeedbackTypes { get; set; }
        public virtual DbSet<PublicFeedback> PublicFeedback { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Restriction> Restriction { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }
        public virtual DbSet<SensorTemplates> SensorTemplates { get; set; }
        public virtual DbSet<ServiceProperties> ServiceProperties { get; set; }
        public virtual DbSet<ServiceRepository> ServiceRepository { get; set; }
        public virtual DbSet<Thing> Thing { get; set; }
        public virtual DbSet<ThingConnections> ThingConnections { get; set; }
        public virtual DbSet<ThingRestrictions> ThingRestrictions { get; set; }
        public virtual DbSet<ThingServices> ThingServices { get; set; }
        public virtual DbSet<ThingTemplates> ThingTemplates { get; set; }
        public virtual DbSet<ThingToPhoneid> ThingToPhoneid { get; set; }
        public virtual DbSet<UserAuth> UserAuth { get; set; }
        public virtual DbSet<UserAuthtoken> UserAuthtoken { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!optionsBuilder.IsConfigured)
            {

                if (ASPNETCORE_ENVIRONMENT == "Development")
                    optionsBuilder.UseNpgsql(COPUpdater.settings.ConnectionString);
                else
                    optionsBuilder.UseNpgsql(COPUpdater.settings.ConnectionString);


            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.Entity<Action>(entity =>
            {
                entity.ToTable("action", "monica");

                entity.ForNpgsqlHasComment("The required user actions for a report to be handled");

                entity.HasIndex(e => e.Personid)
                    .HasName("idx_action_0");

                entity.HasIndex(e => e.Reportid)
                    .HasName("idx_action");

                entity.Property(e => e.Actionid)
                    .HasColumnName("actionid")
                    .HasDefaultValueSql("nextval('monica.action_actionid_seq1'::regclass)");

                entity.Property(e => e.Actiontime).HasColumnName("actiontime");

                entity.Property(e => e.Actiontype).HasColumnName("actiontype");

                entity.Property(e => e.Actortype).HasColumnName("actortype");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Reportid).HasColumnName("reportid");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("character varying(500)");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Action)
                    .HasForeignKey(d => d.Personid)
                    .HasConstraintName("fk_action_person");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.Action)
                    .HasForeignKey(d => d.Reportid)
                    .HasConstraintName("fk_action_report");
            });

            modelBuilder.Entity<AdminLayer>(entity =>
            {
                entity.HasKey(e => e.Layerid);

                entity.ToTable("admin_layer", "monica");

                entity.Property(e => e.Layerid)
                    .HasColumnName("layerid")
                    .HasDefaultValueSql("nextval('monica.admin_layer_layerid_seq1'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");
            });

            modelBuilder.Entity<AdminLayerDets>(entity =>
            {
                entity.HasKey(e => e.Layerdetid);

                entity.ToTable("admin_layer_dets", "monica");

                entity.HasIndex(e => e.Layerid)
                    .HasName("idx_admin_layer_dets_layerid");

                entity.Property(e => e.Layerdetid)
                    .HasColumnName("layerdetid")
                    .HasDefaultValueSql("nextval('monica.admin_layer_dets_layerdetid_seq1'::regclass)");

                entity.Property(e => e.Layerid).HasColumnName("layerid");

                entity.Property(e => e.Obspropertyids)
                    .HasColumnName("obspropertyids")
                    .HasColumnType("character varying(2500)");

                entity.Property(e => e.Thingids)
                    .HasColumnName("thingids")
                    .HasColumnType("character varying(2500)");

                entity.HasOne(d => d.Layer)
                    .WithMany(p => p.AdminLayerDets)
                    .HasForeignKey(d => d.Layerid)
                    .HasConstraintName("fk_admin_layer_dets");
            });

            modelBuilder.Entity<AdminMenu>(entity =>
            {
                entity.HasKey(e => e.Menuid);

                entity.ToTable("admin_menu", "monica");

                entity.Property(e => e.Menuid)
                    .HasColumnName("menuid")
                    .HasDefaultValueSql("nextval('monica.admin_menu_menuid_seq1'::regclass)");

                entity.Property(e => e.Const)
                    .IsRequired()
                    .HasColumnName("const")
                    .HasColumnType("character varying(100)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");
            });

            modelBuilder.Entity<AdminMenuRoles>(entity =>
            {
                entity.HasKey(e => new { e.Menuid, e.Roleid });

                entity.ToTable("admin_menu_roles", "monica");

                entity.HasIndex(e => e.Menuid)
                    .HasName("idx_admin_menu_roles_menuid");

                entity.HasIndex(e => e.Roleid)
                    .HasName("idx_admin_menu_roles_roleid");

                entity.Property(e => e.Menuid).HasColumnName("menuid");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.Property(e => e.Hasaccess).HasColumnName("hasaccess");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.AdminMenuRoles)
                    .HasForeignKey(d => d.Menuid)
                    .HasConstraintName("fk_admin_menu_roles_admin_menu");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AdminMenuRoles)
                    .HasForeignKey(d => d.Roleid)
                    .HasConstraintName("fk_admin_menu_roles_role");
            });

            modelBuilder.Entity<Datastream>(entity =>
            {
                entity.ToTable("datastream", "monica");

                entity.ForNpgsqlHasComment("Groups a collection of Observations measuring the same ObservedProperty and produced by the same Sensor");

                entity.HasIndex(e => e.Observedpropertyid)
                    .HasName("idx_datastream_0");

                entity.HasIndex(e => e.Sensorid)
                    .HasName("idx_datastream");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_datastream_1");

                entity.Property(e => e.Datastreamid)
                    .HasColumnName("datastreamid")
                    .HasDefaultValueSql("nextval('monica.datastream_datastreamid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Observedpropertyid).HasColumnName("observedpropertyid");

                entity.Property(e => e.Sensorid).HasColumnName("sensorid");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.Property(e => e.Unitofmeasurement)
                    .IsRequired()
                    .HasColumnName("unitofmeasurement")
                    .HasColumnType("json");

                entity.HasOne(d => d.Observedproperty)
                    .WithMany(p => p.Datastream)
                    .HasForeignKey(d => d.Observedpropertyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_datastream_observedproperty");

                entity.HasOne(d => d.Sensor)
                    .WithMany(p => p.Datastream)
                    .HasForeignKey(d => d.Sensorid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_datastream_sensor");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.Datastream)
                    .HasForeignKey(d => d.Thingid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_datastream_thing");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event", "monica");

                entity.ForNpgsqlHasComment("The events that use the sensor things that are registered per location");

                entity.Property(e => e.Eventid)
                    .HasColumnName("eventid")
                    .HasDefaultValueSql("nextval('monica.event_eventid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Fromdate).HasColumnName("fromdate");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Todate).HasColumnName("todate");

                entity.Property(e => e.Zoom).HasColumnName("zoom");
            });

            modelBuilder.Entity<EventLocations>(entity =>
            {
                entity.HasKey(e => new { e.Eventid, e.Locationid });

                entity.ToTable("event_locations", "monica");

                entity.ForNpgsqlHasComment("The connected root location per event");

                entity.HasIndex(e => e.Eventid)
                    .HasName("idx_event_locations_0");

                entity.HasIndex(e => e.Locationid)
                    .HasName("idx_event_locations_1");

                entity.Property(e => e.Eventid).HasColumnName("eventid");

                entity.Property(e => e.Locationid).HasColumnName("locationid");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventLocations)
                    .HasForeignKey(d => d.Eventid)
                    .HasConstraintName("fk_event_locations_event");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.EventLocations)
                    .HasForeignKey(d => d.Locationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_event_locations_location");
            });

            modelBuilder.Entity<EventOrganizations>(entity =>
            {
                entity.HasKey(e => new { e.Eventid, e.Organizationid });

                entity.ToTable("event_organizations", "monica");

                entity.ForNpgsqlHasComment("The connected organizations per event");

                entity.HasIndex(e => e.Eventid)
                    .HasName("idx_event_organizations_0");

                entity.HasIndex(e => e.Organizationid)
                    .HasName("idx_event_organizations_1");

                entity.Property(e => e.Eventid).HasColumnName("eventid");

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventOrganizations)
                    .HasForeignKey(d => d.Eventid)
                    .HasConstraintName("fk_event_organizations_event");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.EventOrganizations)
                    .HasForeignKey(d => d.Organizationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_event_organizations_1");
            });

            modelBuilder.Entity<EventServicesProperties>(entity =>
            {
                entity.HasKey(e => e.Eventservicepropertyid);

                entity.ToTable("event_services_properties", "monica");

                entity.ForNpgsqlHasComment("The event's configuration of the various properties of service per type");

                entity.HasIndex(e => e.Eventserviceid)
                    .HasName("idx_event_services_properties");

                entity.Property(e => e.Eventservicepropertyid)
                    .HasColumnName("eventservicepropertyid")
                    .HasDefaultValueSql("nextval('monica.event_services_properties_eventservicepropertyid_seq1'::regclass)");

                entity.Property(e => e.Eventserviceid).HasColumnName("eventserviceid");

                entity.Property(e => e.Metadata)
                    .IsRequired()
                    .HasColumnName("metadata");

                entity.Property(e => e.Propertytypeid).HasColumnName("propertytypeid");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.ToTable("facility", "monica");

                entity.Property(e => e.Facilityid)
                    .HasColumnName("facilityid")
                    .HasDefaultValueSql("nextval('monica.facility_facilityid_seq'::regclass)");

                entity.Property(e => e.Facilitytype).HasColumnName("facilitytype");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(500)");
            });

            modelBuilder.Entity<FeatureOfInterest>(entity =>
            {
                entity.ToTable("feature_of_interest", "monica");

                entity.Property(e => e.Featureofinterestid)
                    .HasColumnName("featureofinterestid")
                    .HasDefaultValueSql("nextval('monica.feature_of_interest_featureofinterestid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");
            });

            modelBuilder.Entity<Fffriends>(entity =>
            {
                entity.HasKey(e => new { e.Friend1, e.Friend2 });

                entity.ToTable("FFFriends", "monica");

                entity.Property(e => e.Friend1).HasColumnType("character varying(30)");

                entity.Property(e => e.Friend2).HasColumnType("character varying(30)");
            });

            modelBuilder.Entity<FriendfinderReg>(entity =>
            {
                entity.ToTable("FriendfinderReg", "monica");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("character varying(30)")
                    .ValueGeneratedNever();

                entity.Property(e => e.NickName).HasColumnType("character varying(100)");
            });

            modelBuilder.Entity<Incident>(entity =>
            {
                entity.ToTable("incident", "monica");

                entity.Property(e => e.Incidentid)
                    .HasColumnName("incidentid")
                    .HasDefaultValueSql("nextval('monica.incident_incidentid_seq'::regclass)");

                entity.Property(e => e.AdditionalMedia)
                    .HasColumnType("character varying(200)")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.AdditionalMediaType)
                    .HasColumnName("AdditionalMediaTYpe")
                    .HasColumnType("character varying(200)")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Area)
                    .HasColumnType("character varying(200)")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying(12000)");

                entity.Property(e => e.Incidenttime)
                    .HasColumnName("incidenttime")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Interventionplan)
                    .HasColumnName("interventionplan")
                    .HasColumnType("character varying(32000)");

                entity.Property(e => e.PhoneNumber).HasColumnType("character varying(100)");

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasColumnType("character varying(255)");

                entity.Property(e => e.Prio)
                    .HasColumnName("prio")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Probability)
                    .HasColumnName("probability")
                    .HasDefaultValueSql("0.0");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("character varying(100)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("character varying(200)");

                entity.Property(e => e.WearablePhysicalId).HasColumnType("character varying(100)");
            });

            modelBuilder.Entity<InterventionActions>(entity =>
            {
                entity.HasKey(e => e.Interventionactionid);

                entity.ToTable("intervention_actions", "monica");

                entity.HasIndex(e => e.Interventionplanid)
                    .HasName("idx_intervention_actions_interventionplanid");

                entity.Property(e => e.Interventionactionid)
                    .HasColumnName("interventionactionid")
                    .HasDefaultValueSql("nextval('monica.intervention_actions_interventionactionid_seq'::regclass)");

                entity.Property(e => e.Interventionplanid).HasColumnName("interventionplanid");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(100)");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.HasOne(d => d.Interventionplan)
                    .WithMany(p => p.InterventionActions)
                    .HasForeignKey(d => d.Interventionplanid)
                    .HasConstraintName("fk_intervention_actions");
            });

            modelBuilder.Entity<InterventionPlan>(entity =>
            {
                entity.ToTable("intervention_plan", "monica");

                entity.Property(e => e.Interventionplanid)
                    .HasColumnName("interventionplanid")
                    .HasDefaultValueSql("nextval('monica.intervention_plan_interventionplanid_seq'::regclass)");

                entity.Property(e => e.Interventiontype).HasColumnName("interventiontype");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(100)");
            });

            modelBuilder.Entity<LatestObservation>(entity =>
            {
                entity.HasKey(e => new { e.Thingid, e.Datastreamid });

                entity.ToTable("latest_observation", "monica");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.Property(e => e.Datastreamid)
                    .HasColumnName("datastreamid")
                    .HasColumnType("character varying(400)");

                entity.Property(e => e.Locationid).HasColumnName("locationid");

                entity.Property(e => e.Observationresult).HasColumnName("observationresult");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Phenomentime).HasColumnName("phenomentime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("character varying(30)")
                    .HasDefaultValueSql("''::character varying");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location", "monica");

                entity.ForNpgsqlHasComment("The hierarchical tree of locations");

                entity.HasIndex(e => e.Locationtemplateid)
                    .HasName("idx_location_locationtemplateid");

                entity.Property(e => e.Locationid)
                    .HasColumnName("locationid")
                    .HasDefaultValueSql("nextval('monica.location_locationid_seq1'::regclass)");

                entity.Property(e => e.Boundingpolygon)
                    .HasColumnName("boundingpolygon")
                    .HasColumnType("character varying(2500)");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Locationtemplateid).HasColumnName("locationtemplateid");

                entity.Property(e => e.Locationtypeid).HasColumnName("locationtypeid");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Parentid).HasColumnName("parentid");

                entity.HasOne(d => d.Locationtemplate)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.Locationtemplateid)
                    .HasConstraintName("fk_location_location_templates");
            });

            modelBuilder.Entity<LocationRestrictions>(entity =>
            {
                entity.HasKey(e => e.Locationrestrictionid);

                entity.ToTable("location_restrictions", "monica");

                entity.HasIndex(e => e.Locationid)
                    .HasName("idx_location_restrictions_locationid");

                entity.HasIndex(e => e.Restrictionid)
                    .HasName("idx_location_restrictions_restrictionid");

                entity.Property(e => e.Locationrestrictionid)
                    .HasColumnName("locationrestrictionid")
                    .HasDefaultValueSql("nextval('monica.location_restrictions_locationrestrictionid_seq'::regclass)");

                entity.Property(e => e.Locationid).HasColumnName("locationid");

                entity.Property(e => e.Restrictionid).HasColumnName("restrictionid");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationRestrictions)
                    .HasForeignKey(d => d.Locationid)
                    .HasConstraintName("fk_location_restrictions");

                entity.HasOne(d => d.Restriction)
                    .WithMany(p => p.LocationRestrictions)
                    .HasForeignKey(d => d.Restrictionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_location_restrictions_1");
            });

            modelBuilder.Entity<LocationServices>(entity =>
            {
                entity.HasKey(e => new { e.Locationid, e.Serviceid });

                entity.ToTable("location_services", "monica");

                entity.ForNpgsqlHasComment("The enabled services per location");

                entity.HasIndex(e => e.Locationid)
                    .HasName("idx_location_services_0");

                entity.HasIndex(e => e.Serviceid)
                    .HasName("idx_location_services_1");

                entity.Property(e => e.Locationid).HasColumnName("locationid");

                entity.Property(e => e.Serviceid).HasColumnName("serviceid");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationServices)
                    .HasForeignKey(d => d.Locationid)
                    .HasConstraintName("fk_location_services_location");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.LocationServices)
                    .HasForeignKey(d => d.Serviceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_location_services");
            });

            modelBuilder.Entity<LocationTemplates>(entity =>
            {
                entity.HasKey(e => e.Locationtemplateid);

                entity.ToTable("location_templates", "monica");

                entity.Property(e => e.Locationtemplateid)
                    .HasColumnName("locationtemplateid")
                    .HasDefaultValueSql("nextval('monica.location_templates_locationtemplateid_seq1'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.PublicAccess).HasColumnName("public_access");

                entity.Property(e => e.Templateimage).HasColumnName("templateimage");
            });

            modelBuilder.Entity<LocationThings>(entity =>
            {
                entity.HasKey(e => new { e.Timepoint, e.Thingid, e.Locationid });

                entity.ToTable("location_things", "monica");

                entity.ForNpgsqlHasComment("The available things per location");

                entity.HasIndex(e => e.Locationid)
                    .HasName("idx_location_things");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_location_things_0");

                entity.Property(e => e.Timepoint).HasColumnName("timepoint");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.Property(e => e.Locationid).HasColumnName("locationid");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationThings)
                    .HasForeignKey(d => d.Locationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_locationthings_location");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.LocationThings)
                    .HasForeignKey(d => d.Thingid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_locationthings_thing");
            });

            modelBuilder.Entity<Observation>(entity =>
            {
                entity.ToTable("observation", "monica");

                entity.ForNpgsqlHasComment("The act of measuring or otherwise determining the value of a property");

                entity.HasIndex(e => e.Datastreamid)
                    .HasName("idx_observation");

                entity.HasIndex(e => e.Featureofinterestid)
                    .HasName("idx_observation_featureofinterestid");

                entity.Property(e => e.Observationid)
                    .HasColumnName("observationid")
                    .HasDefaultValueSql("nextval('monica.observation_observationid_seq1'::regclass)");

                entity.Property(e => e.Datastreamid).HasColumnName("datastreamid");

                entity.Property(e => e.Featureofinterestid).HasColumnName("featureofinterestid");

                entity.Property(e => e.Observationresult)
                    .IsRequired()
                    .HasColumnName("observationresult");

                entity.Property(e => e.Phenomenontime).HasColumnName("phenomenontime");

                entity.Property(e => e.Resulttime).HasColumnName("resulttime");

                entity.HasOne(d => d.Datastream)
                    .WithMany(p => p.Observation)
                    .HasForeignKey(d => d.Datastreamid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_observation_datastream");

                entity.HasOne(d => d.Featureofinterest)
                    .WithMany(p => p.Observation)
                    .HasForeignKey(d => d.Featureofinterestid)
                    .HasConstraintName("fk_observation_1");
            });

            modelBuilder.Entity<ObservedProperty>(entity =>
            {
                entity.ToTable("observed_property", "monica");

                entity.ForNpgsqlHasComment("Specifies the phenomenon of an Observation");

                entity.Property(e => e.Observedpropertyid)
                    .HasColumnName("observedpropertyid")
                    .HasDefaultValueSql("nextval('monica.observed_property_observedpropertyid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organization", "monica");

                entity.ForNpgsqlHasComment("The organization the holds an event");

                entity.Property(e => e.Organizationid)
                    .HasColumnName("organizationid")
                    .HasDefaultValueSql("nextval('monica.organization_organizationid_seq1'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person", "monica");

                entity.ForNpgsqlHasComment("The registered persons of the organization");

                entity.HasIndex(e => e.Organizationid)
                    .HasName("idx_person");

                entity.Property(e => e.Personid)
                    .HasColumnName("personid")
                    .HasDefaultValueSql("nextval('monica.person_personid_seq1'::regclass)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("character varying(100)");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasColumnName("fullname")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Isactive).HasColumnName("isactive");

                entity.Property(e => e.Organizationid).HasColumnName("organizationid");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("character varying(100)");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.Organizationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_person_organization");
            });

            modelBuilder.Entity<PersonRoles>(entity =>
            {
                entity.HasKey(e => new { e.Personid, e.Roleid });

                entity.ToTable("person_roles", "monica");

                entity.ForNpgsqlHasComment("The connected roles per person");

                entity.HasIndex(e => e.Personid)
                    .HasName("idx_person_roles_0");

                entity.HasIndex(e => e.Roleid)
                    .HasName("idx_person_roles_1");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonRoles)
                    .HasForeignKey(d => d.Personid)
                    .HasConstraintName("fk_person_roles_person");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PersonRoles)
                    .HasForeignKey(d => d.Roleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_person_roles_role");
            });

            modelBuilder.Entity<PersonThings>(entity =>
            {
                entity.HasKey(e => new { e.Personid, e.Thingid });

                entity.ToTable("person_things", "monica");

                entity.HasIndex(e => e.Personid)
                    .HasName("idx_person_thing_personid");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_person_thing_thingid");

                entity.Property(e => e.Personid).HasColumnName("personid");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.Property(e => e.Timepoint).HasColumnName("timepoint");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonThings)
                    .HasForeignKey(d => d.Personid)
                    .HasConstraintName("fk_person_thing_person");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.PersonThings)
                    .HasForeignKey(d => d.Thingid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_person_thing_thing");
            });

            modelBuilder.Entity<ProAcousticFeedback>(entity =>
            {
                entity.ToTable("pro_acoustic_feedback", "monica");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('monica.pro_acoustic_feedback_id_seq'::regclass)");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Phoneid)
                    .IsRequired()
                    .HasColumnName("phoneid")
                    .HasColumnType("character varying(80)");

                entity.Property(e => e.ProAcousticFeedbackType).HasColumnName("pro_acoustic_feedback_type");

                entity.Property(e => e.ReportType).HasColumnName("report_type");

                entity.Property(e => e.Textmessage)
                    .HasColumnName("textmessage")
                    .HasColumnType("character varying(32000)");

                entity.HasOne(d => d.ProAcousticFeedbackTypeNavigation)
                    .WithMany(p => p.ProAcousticFeedback)
                    .HasForeignKey(d => d.ProAcousticFeedbackType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pro_acoustic_feedback_fk");
            });

            modelBuilder.Entity<ProAcousticFeedbackTypes>(entity =>
            {
                entity.ToTable("pro_acoustic_feedback_types", "monica");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('monica.pro_acoustic_feedback_types_id_seq'::regclass)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying(32000)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying(255)");
            });

            modelBuilder.Entity<PublicFeedback>(entity =>
            {
                entity.HasKey(e => new { e.Phoneid, e.FeedbackType });

                entity.ToTable("public_feedback", "monica");

                entity.Property(e => e.Phoneid)
                    .HasColumnName("phoneid")
                    .HasColumnType("character varying(255)");

                entity.Property(e => e.FeedbackType)
                    .HasColumnName("feedback_type")
                    .HasColumnType("character varying(255)");

                entity.Property(e => e.FeedbackValue).HasColumnName("feedback_value");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report", "monica");

                entity.ForNpgsqlHasComment("Automatically or manually generated reports concerning various types of possible incidents");

                entity.HasIndex(e => e.Eventid)
                    .HasName("idx_report");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_report_thingid");

                entity.Property(e => e.Reportid)
                    .HasColumnName("reportid")
                    .HasDefaultValueSql("nextval('monica.report_reportid_seq1'::regclass)");

                entity.Property(e => e.Allowmultiuserhandle).HasColumnName("allowmultiuserhandle");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Eventid).HasColumnName("eventid");

                entity.Property(e => e.Evidence).HasColumnName("evidence");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.Reportcode)
                    .HasColumnName("reportcode")
                    .HasColumnType("character varying(50)");

                entity.Property(e => e.Reporttime).HasColumnName("reporttime");

                entity.Property(e => e.Reporttype).HasColumnName("reporttype");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Eventid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_report_event");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Thingid)
                    .HasConstraintName("fk_report_thing");
            });

            modelBuilder.Entity<Restriction>(entity =>
            {
                entity.ToTable("restriction", "monica");

                entity.Property(e => e.Restrictionid)
                    .HasColumnName("restrictionid")
                    .HasDefaultValueSql("nextval('monica.restriction_restrictionid_seq1'::regclass)");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Typeid).HasColumnName("typeid");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role", "monica");

                entity.ForNpgsqlHasComment("Available person roles");

                entity.Property(e => e.Roleid)
                    .HasColumnName("roleid")
                    .HasDefaultValueSql("nextval('monica.role_roleid_seq1'::regclass)");

                entity.Property(e => e.Const)
                    .IsRequired()
                    .HasColumnName("const")
                    .HasColumnType("character varying(50)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("character varying(250)");
            });

            modelBuilder.Entity<Sensor>(entity =>
            {
                entity.ToTable("sensor", "monica");

                entity.ForNpgsqlHasComment("An instrument that observes a property or phenomenon with the goal of producing an estimate of the value of the property");

                entity.HasIndex(e => e.Templateid)
                    .HasName("idx_sensor");

                entity.Property(e => e.Sensorid)
                    .HasColumnName("sensorid")
                    .HasDefaultValueSql("nextval('monica.sensor_sensorid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Metadata).HasColumnName("metadata");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Templateid).HasColumnName("templateid");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Sensor)
                    .HasForeignKey(d => d.Templateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sensor_sensor_templates");
            });

            modelBuilder.Entity<SensorTemplates>(entity =>
            {
                entity.HasKey(e => e.Sensortemplateid);

                entity.ToTable("sensor_templates", "monica");

                entity.ForNpgsqlHasComment("The predifined templates of sensors with the schema of properties");

                entity.Property(e => e.Sensortemplateid)
                    .HasColumnName("sensortemplateid")
                    .HasDefaultValueSql("nextval('monica.sensor_templates_sensortemplateid_seq1'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Sensortype).HasColumnName("sensortype");

                entity.Property(e => e.Templateschema)
                    .IsRequired()
                    .HasColumnName("templateschema");
            });

            modelBuilder.Entity<ServiceProperties>(entity =>
            {
                entity.HasKey(e => e.Servicepropertyid);

                entity.ToTable("service_properties", "monica");

                entity.ForNpgsqlHasComment("The configuration of the various properties of service per type");

                entity.HasIndex(e => e.Serviceid)
                    .HasName("idx_service_properties");

                entity.Property(e => e.Servicepropertyid)
                    .HasColumnName("servicepropertyid")
                    .HasDefaultValueSql("nextval('monica.service_properties_servicepropertyid_seq1'::regclass)");

                entity.Property(e => e.Metadata)
                    .IsRequired()
                    .HasColumnName("metadata");

                entity.Property(e => e.Propertytypeid).HasColumnName("propertytypeid");

                entity.Property(e => e.Serviceid).HasColumnName("serviceid");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceProperties)
                    .HasForeignKey(d => d.Serviceid)
                    .HasConstraintName("fk_service_properties");
            });

            modelBuilder.Entity<ServiceRepository>(entity =>
            {
                entity.HasKey(e => e.Serviceid);

                entity.ToTable("service_repository", "monica");

                entity.ForNpgsqlHasComment("The repository of the services that are ready for activation by locations and events");

                entity.Property(e => e.Serviceid)
                    .HasColumnName("serviceid")
                    .HasDefaultValueSql("nextval('monica.service_repository_serviceid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(500)");
            });

            modelBuilder.Entity<Thing>(entity =>
            {
                entity.ToTable("thing", "monica");

                entity.ForNpgsqlHasComment("An object of the physical world or the information world that is capable of being identified and integrated into communication networks");

                entity.HasIndex(e => e.Templateid)
                    .HasName("idx_thing_templateid");

                entity.Property(e => e.Thingid)
                    .HasColumnName("thingid")
                    .HasDefaultValueSql("nextval('monica.thing_thingid_seq1'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Ogcid)
                    .HasColumnName("ogcid")
                    .HasColumnType("character varying(100)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Templateid).HasColumnName("templateid");

                entity.Property(e => e.Templatevalues).HasColumnName("templatevalues");

                entity.Property(e => e.Thingtype)
                    .HasColumnName("thingtype")
                    .ForNpgsqlHasComment("0:physical, 1:virtual");

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Thing)
                    .HasForeignKey(d => d.Templateid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_thing_thing_templates");
            });

            modelBuilder.Entity<ThingConnections>(entity =>
            {
                entity.HasKey(e => e.Thingconid);

                entity.ToTable("thing_connections", "monica");

                entity.HasIndex(e => e.Refthingid)
                    .HasName("idx_thing_connections_refthingid");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_thing_connections_thingid");

                entity.Property(e => e.Thingconid)
                    .HasColumnName("thingconid")
                    .HasDefaultValueSql("nextval('monica.thing_connections_thingconid_seq1'::regclass)");

                entity.Property(e => e.Refthingid).HasColumnName("refthingid");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.HasOne(d => d.Refthing)
                    .WithMany(p => p.ThingConnectionsRefthing)
                    .HasForeignKey(d => d.Refthingid)
                    .HasConstraintName("fk_thing_connections_thing_2");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.ThingConnectionsThing)
                    .HasForeignKey(d => d.Thingid)
                    .HasConstraintName("fk_thing_connections_thing_1");
            });

            modelBuilder.Entity<ThingRestrictions>(entity =>
            {
                entity.HasKey(e => e.Thingrestrictionid);

                entity.ToTable("thing_restrictions", "monica");

                entity.HasIndex(e => e.Restrictionid)
                    .HasName("idx_thing_restrictions_restrictionid");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_thing_restrictions_thingid");

                entity.Property(e => e.Thingrestrictionid)
                    .HasColumnName("thingrestrictionid")
                    .HasDefaultValueSql("nextval('monica.thing_restrictions_thingrestrictionid_seq1'::regclass)");

                entity.Property(e => e.Restrictionid).HasColumnName("restrictionid");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.HasOne(d => d.Restriction)
                    .WithMany(p => p.ThingRestrictions)
                    .HasForeignKey(d => d.Restrictionid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_thing_restrictions");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.ThingRestrictions)
                    .HasForeignKey(d => d.Thingid)
                    .HasConstraintName("fk_thing_restrictions_thing");
            });

            modelBuilder.Entity<ThingServices>(entity =>
            {
                entity.HasKey(e => e.Thingserviceid);

                entity.ToTable("thing_services", "monica");

                entity.HasIndex(e => e.Serviceid)
                    .HasName("idx_thing_services_serviceid");

                entity.HasIndex(e => e.Thingid)
                    .HasName("idx_thing_services_thingid");

                entity.Property(e => e.Thingserviceid)
                    .HasColumnName("thingserviceid")
                    .HasDefaultValueSql("nextval('monica.thing_services_thingserviceid_seq1'::regclass)");

                entity.Property(e => e.Serviceid).HasColumnName("serviceid");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ThingServices)
                    .HasForeignKey(d => d.Serviceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_thing_services_fk2");

                entity.HasOne(d => d.Thing)
                    .WithMany(p => p.ThingServices)
                    .HasForeignKey(d => d.Thingid)
                    .HasConstraintName("fk_thing_services_thing");
            });

            modelBuilder.Entity<ThingTemplates>(entity =>
            {
                entity.HasKey(e => e.Thingtemplateid);

                entity.ToTable("thing_templates", "monica");

                entity.Property(e => e.Thingtemplateid)
                    .HasColumnName("thingtemplateid")
                    .HasDefaultValueSql("nextval('monica.thing_templates_thingtemplateid_seq1'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying(250)");

                entity.Property(e => e.Templateschema).HasColumnName("templateschema");
            });

            modelBuilder.Entity<ThingToPhoneid>(entity =>
            {
                entity.HasKey(e => new { e.Thingid, e.Phoneid });

                entity.ToTable("thing_to_phoneid", "monica");

                entity.Property(e => e.Thingid).HasColumnName("thingid");

                entity.Property(e => e.Phoneid)
                    .HasColumnName("phoneid")
                    .HasColumnType("character varying(300)");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasColumnType("character varying(100)");
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.ToTable("user_auth", "monica");

                entity.ForNpgsqlHasComment("The authentication info for the people that are registered as users");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("character varying(100)");

                entity.Property(e => e.Userpassword)
                    .IsRequired()
                    .HasColumnName("userpassword")
                    .HasColumnType("character varying(100)");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserAuth)
                    .HasForeignKey<UserAuth>(d => d.Userid)
                    .HasConstraintName("fk_user_auth_person");
            });

            modelBuilder.Entity<UserAuthtoken>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Authtoken, e.Dateissued });

                entity.ToTable("user_authtoken", "monica");

                entity.ForNpgsqlHasComment("The generated authentication tokens of each user");

                entity.HasIndex(e => e.Userid)
                    .HasName("idx_user_authtoken");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Authtoken)
                    .HasColumnName("authtoken")
                    .HasColumnType("character varying(128)");

                entity.Property(e => e.Dateissued).HasColumnName("dateissued");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAuthtoken)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("fk_user_authtoken_user_auth");
            });

            modelBuilder.HasSequence("action_actionid_seq");

            modelBuilder.HasSequence("action_actionid_seq1");

            modelBuilder.HasSequence("admin_layer_dets_layerdetid_seq");

            modelBuilder.HasSequence("admin_layer_dets_layerdetid_seq1");

            modelBuilder.HasSequence("admin_layer_layerid_seq");

            modelBuilder.HasSequence("admin_layer_layerid_seq1");

            modelBuilder.HasSequence("admin_menu_menuid_seq");

            modelBuilder.HasSequence("admin_menu_menuid_seq1");

            modelBuilder.HasSequence("datastream_datastreamid_seq");

            modelBuilder.HasSequence("datastream_datastreamid_seq1");

            modelBuilder.HasSequence("event_eventid_seq");

            modelBuilder.HasSequence("event_eventid_seq1");

            modelBuilder.HasSequence("event_services_eventserviceid_seq");

            modelBuilder.HasSequence("event_services_properties_eventservicepropertyid_seq");

            modelBuilder.HasSequence("event_services_properties_eventservicepropertyid_seq1");

            modelBuilder.HasSequence("facility_facilityid_seq");

            modelBuilder.HasSequence("feature_of_interest_featureofinterestid_seq");

            modelBuilder.HasSequence("feature_of_interest_featureofinterestid_seq1");

            modelBuilder.HasSequence("incident_incidentid_seq");

            modelBuilder.HasSequence("intervention_actions_interventionactionid_seq");

            modelBuilder.HasSequence("intervention_plan_interventionplanid_seq");

            modelBuilder.HasSequence("location_locationid_seq");

            modelBuilder.HasSequence("location_locationid_seq1");

            modelBuilder.HasSequence("location_restrictions_locationrestrictionid_seq");

            modelBuilder.HasSequence("location_templates_locationtemplateid_seq");

            modelBuilder.HasSequence("location_templates_locationtemplateid_seq1");

            modelBuilder.HasSequence("observation_observationid_seq");

            modelBuilder.HasSequence("observation_observationid_seq1");

            modelBuilder.HasSequence("observed_property_observedpropertyid_seq");

            modelBuilder.HasSequence("observed_property_observedpropertyid_seq1");

            modelBuilder.HasSequence("organization_organizationid_seq");

            modelBuilder.HasSequence("organization_organizationid_seq1");

            modelBuilder.HasSequence("person_personid_seq");

            modelBuilder.HasSequence("person_personid_seq1");

            modelBuilder.HasSequence("pro_acoustic_feedback_id_seq");

            modelBuilder.HasSequence("pro_acoustic_feedback_types_id_seq");

            modelBuilder.HasSequence("report_reportid_seq");

            modelBuilder.HasSequence("report_reportid_seq1");

            modelBuilder.HasSequence("restriction_restrictionid_seq");

            modelBuilder.HasSequence("restriction_restrictionid_seq1");

            modelBuilder.HasSequence("role_roleid_seq");

            modelBuilder.HasSequence("role_roleid_seq1");

            modelBuilder.HasSequence("sensor_sensorid_seq");

            modelBuilder.HasSequence("sensor_sensorid_seq1");

            modelBuilder.HasSequence("sensor_templates_sensortemplateid_seq");

            modelBuilder.HasSequence("sensor_templates_sensortemplateid_seq1");

            modelBuilder.HasSequence("service_properties_servicepropertyid_seq");

            modelBuilder.HasSequence("service_properties_servicepropertyid_seq1");

            modelBuilder.HasSequence("service_repository_serviceid_seq");

            modelBuilder.HasSequence("service_repository_serviceid_seq1");

            modelBuilder.HasSequence("thing_connections_thingconid_seq");

            modelBuilder.HasSequence("thing_connections_thingconid_seq1");

            modelBuilder.HasSequence("thing_restrictions_thingrestrictionid_seq");

            modelBuilder.HasSequence("thing_restrictions_thingrestrictionid_seq1");

            modelBuilder.HasSequence("thing_services_thingserviceid_seq");

            modelBuilder.HasSequence("thing_services_thingserviceid_seq1");

            modelBuilder.HasSequence("thing_templates_thingtemplateid_seq");

            modelBuilder.HasSequence("thing_templates_thingtemplateid_seq1");

            modelBuilder.HasSequence("thing_thingid_seq");

            modelBuilder.HasSequence("thing_thingid_seq1");
        }
    }
}