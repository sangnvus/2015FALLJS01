"use strict";

service.service("AdminCategoryService", function ($http) {

    //Read all Categories for administrator.
    this.getCategories = function () {
        return $http.get("../api/CategoryApi/GetCategoriesForAdmin");
    };


    // Change status of category
    this.changeCategoryStatus = function (id) {
        var request = $http({
            method: 'put',
            url: '../api/CategoryApi/ChangeCategoryStatus',
            params: {
                id: id
            }
        });
        return request;
    }

    // Add new category
    this.addNewCategory = function (category) {
        var request = $http({
            method: 'post',
            url: '../api/CategoryApi/AddNewCategory',
            data: category
        });
        return request;
    }

    // Add new category
    this.deleteCategory = function (id) {
        var request = $http({
            method: 'delete',
            url: '../api/CategoryApi/DeleteCategory',
            params: {
                id: id
            }
        });
        return request;
    }

    // edit category
    this.editCategory = function (category) {
        var request = $http({
            method: 'put',
            url: '../api/CategoryApi/EditCategory',
            data: category
        });
        return request;
    }
});