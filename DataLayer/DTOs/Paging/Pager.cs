using System;

namespace DataLayer.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageId , int allEntitiesCount , int takeEntities , int howManyAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)takeEntities));   
            var basePaging = new BasePaging();  
            basePaging.PageId = pageId;
            basePaging.AllEntitiesCount = allEntitiesCount;
            basePaging.TakeEntity = takeEntities;
            basePaging.HomManyPageAfterAndBefore = howManyAfterAndBefore;
            basePaging.SkipEntity = (pageId - 1) * takeEntities;
            basePaging.StartPage = howManyAfterAndBefore - pageId <= 0 ? 1 : howManyAfterAndBefore - pageId;
            basePaging.EndPage = howManyAfterAndBefore + pageId >= pageCount ? pageCount : howManyAfterAndBefore + pageId;
            basePaging.PageCount = pageCount;

            return basePaging;
        }
    }
}
