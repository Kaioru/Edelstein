using System;
using System.Collections.Generic;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Shop;

namespace Edelstein.Service.Shop.Commodities
{
    public class CommodityManager
    {
        private readonly ITemplateManager _templateManager;
        private readonly IDictionary<int, Commodity> _commodities;

        public CommodityManager(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
            _commodities = new Dictionary<int, Commodity>();
        }

        public Commodity Get(int id)
        {
            lock (this)
            {
                if (!_commodities.ContainsKey(id))
                {
                    var t = _templateManager.Get<CommodityTemplate>(id);
                    var m = _templateManager.Get<ModifiedCommodityTemplate>(id);
                    Commodity c = null;

                    if (t != null)
                    {
                        c = new Commodity
                        {
                            SN = id,
                            ItemID = t.ItemID,
                            Count = t.Count,
                            Priority = t.Priority,
                            Price = t.Price,
                            Bonus = t.Bonus,
                            Period = t.Period,
                            ReqPOP = t.ReqPOP,
                            ReqLEV = t.ReqLEV,
                            MaplePoint = t.MaplePoint,
                            Meso = t.Meso,
                            ForPremiumUser = t.ForPremiumUser,
                            Gender = t.Gender,
                            OnSale = t.OnSale,
                            Class = t.Class,
                            Limit = t.Limit,
                            PbCash = t.PbCash,
                            PbPoint = t.PbPoint,
                            PbGift = t.PbGift
                        };

                        var p = _templateManager.Get<CashPackageTemplate>(c.ItemID);

                        if (p != null)
                            c.PackageSN = p.PackageSN;
                    }

                    if (m != null)
                    {
                        if (c == null) c = new Commodity();

                        if (m.ItemID.HasValue) c.ItemID = m.ItemID.Value;
                        if (m.Count.HasValue) c.Count = m.Count.Value;
                        if (m.Priority.HasValue) c.Priority = m.Priority.Value;
                        if (m.Price.HasValue) c.Price = m.Price.Value;
                        if (m.Bonus.HasValue) c.Bonus = m.Bonus.Value;
                        if (m.Period.HasValue) c.Period = m.Period.Value;
                        if (m.ReqPOP.HasValue) c.ReqPOP = m.ReqPOP.Value;
                        if (m.ReqLEV.HasValue) c.ReqLEV = m.ReqLEV.Value;
                        if (m.MaplePoint.HasValue) c.MaplePoint = m.MaplePoint.Value;
                        if (m.Meso.HasValue) c.Meso = m.Meso.Value;
                        if (m.ForPremiumUser.HasValue) c.ForPremiumUser = m.ForPremiumUser.Value;
                        if (m.Gender.HasValue) c.Gender = Convert.ToSByte(m.Gender.Value);
                        if (m.OnSale.HasValue) c.OnSale = m.OnSale.Value;
                        if (m.Class.HasValue) c.Class = m.Class.Value;
                        if (m.Limit.HasValue) c.Limit = m.Limit.Value;
                        if (m.PbCash.HasValue) c.PbCash = m.PbCash.Value;
                        if (m.PbPoint.HasValue) c.PbPoint = m.PbPoint.Value;
                        if (m.PbGift.HasValue) c.PbGift = m.PbGift.Value;
                        if (m.PackageSN != null) c.PackageSN = m.PackageSN;
                    }

                    c.OnSale = _templateManager.Get<NotSaleTemplate>(id) == null;

                    if (c == null) return null;
                    _commodities[id] = c;
                }

                return _commodities[id];
            }
        }
    }
}