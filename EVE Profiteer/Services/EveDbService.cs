using eZet.Eve.EveProfiteer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Services {
    public class EveDbService {

        private EveDbContext db = new EveDbContext();

        public void SetLazyLoad(bool val) {
            db.Configuration.LazyLoadingEnabled = val;
        }

        public Item GetItem(long id) {
            var query = from item in db.Items
                        where item.TypeId == id
                        select item;
            return query.First();
        }

        public List<Item> GetItems() {
            var query = from item in db.Items
                        select item;
            return query.ToList();
        }

        public MarketGroup GetMarketGroup(long id) {
            var query = from g in db.MarketGroups
                        where g.MarketGroupId == id
                        select g;
            return query.First();

        }

        public ICollection<MarketGroup> GetMarketGroups() {
            var query = from g in db.MarketGroups
                        select g;
            
            return query.ToList();
        }

        public IQueryable<MarketGroup> GetRootMarketGroups() {
            var q = from b in db.MarketGroups.Include("SubGroups").Include("SubGroups.Items").Include("SubGroups.SubGroups")
                        .Include("SubGroups.SubGroups.SubGroups").Include("SubGroups.SubGroups.Items")
                        .Include("SubGroups.SubGroups.SubGroups.SubGroups").Include("SubGroups.SubGroups.SubGroups.Items")
                        .Include("SubGroups.SubGroups.SubGroups.SubGroups.SubGroups").Include("SubGroups.SubGroups.SubGroups.SubGroups.Items")
                        .Include("SubGroups.SubGroups.SubGroups.SubGroups.SubGroups.SubGroups").Include("SubGroups.SubGroups.SubGroups.SubGroups.SubGroups.Items")
                        .Include("SubGroups.SubGroups.SubGroups.SubGroups.SubGroups.SubGroups.Items")
                        where b.ParentGroup == null
                        select b;
                db.Configuration.LazyLoadingEnabled = true;
                return q;
        }

        public IQueryable<Region> GetRegions() {
            var query = from row in db.Regions
                        where row.RegionId < 11000001
                        orderby row.RegionName
                        select row;
            return query;
        }
    }
}
