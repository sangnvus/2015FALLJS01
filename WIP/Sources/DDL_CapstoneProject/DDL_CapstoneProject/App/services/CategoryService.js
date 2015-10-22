﻿"use strict";

service.service("CategoryService", function ($http) {

    //Read all Categories  


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

});