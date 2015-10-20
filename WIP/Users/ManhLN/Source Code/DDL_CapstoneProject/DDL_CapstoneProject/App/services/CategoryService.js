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

    this.GetCategoryProjectCount = function () {
        return $http.get('/api/CategoryApi/GetCategoryProjectCount');
    };
});