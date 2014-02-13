using eZet.Eve.EveProfiteer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Services {
    public class EveDbService {

        private EveDbContext db = new EveDbContext();

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
            var query = from b in db.MarketGroups.Include("SubGroups").Include("Items")
                        where b.ParentGroup == null
                        select b;
            return query;
        }
    }
}
