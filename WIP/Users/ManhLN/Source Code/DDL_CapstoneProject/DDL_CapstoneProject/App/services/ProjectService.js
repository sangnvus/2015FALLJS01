"use strict";

app.service('ProjectService', function($http) {
    // Function to create new Project
    this.createProject = function (Project) {
        var request = $http({
            method: 'post',
            url: '/api/ProjectApi/CreateProject',
            data: Project
        });

        return request;
    }

});