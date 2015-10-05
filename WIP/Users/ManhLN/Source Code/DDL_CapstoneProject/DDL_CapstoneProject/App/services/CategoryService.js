"use strict";

service.service("CategoryService", function ($http) {

    //Read all Categories  
    this.getCategories = function () {
        return $http.get("api/CategoryAPI/GetCategories");
    };

});