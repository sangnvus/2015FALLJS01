"use strict";

service.service("CategoryService", function ($http) {

    //Read all Categories  
    this.getCategories = function () {
        return $http.get('/api/CategoryApi/GetCategories');
    };

    //Read all Categories  for create page
    this.GetCategoriesForCreate = function () {
        return $http.get('/api/CategoryApi/GetCategoriesForCreate');
    };

    this.categoryStatistic = function () {
        return $http.get("api/CategoryApi/categoryStatistic");
    };

    this.GetCategoryProjectCount = function () {
        return $http.get('/api/CategoryApi/GetCategoryProjectCount');
    };

    this.getAllCategories = function () {
        return $http.get("api/CategoryApi/getAllCategories");
    }
    this.GetCategories = function (categoryid) {
        return $http.get("api/CategoryApi/GetCategories?categoryid=" + categoryid);
    };

    this.listDataForStatistic = function () {
        return $http.get("api/CategoryApi/listDataForStatistic ");
    };

    //Read all Categories  for create page
    this.GetCategoriesForCreate = function () {
        return $http.get('/api/CategoryApi/GetCategoriesForCreate');
    };
});