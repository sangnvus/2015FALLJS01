"use strict";

service.service("AdminProjectService", function ($http) {

    //Get basic statistic of project for admin
    this.getBasicStatisticProject = function () {
        return $http.get("../api/ProjectApi/AdminGetProjectGeneralInfo");
    };

    //Get pending project list 
    this.getPendingProjectList = function () {
        return $http.get("../api/ProjectApi/GetPendingProjectList");
    };

    //Get project list 
    this.getProjectList = function () {
        return $http.get("../api/ProjectApi/AdminGetProjectList");
    };

    // Function to get a project by ProjectCode
    this.getProjectDetail = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/AdminGetProjectDetail',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to ger a project by ProjectCode
    this.getCommentList = function (code, lastDatetime) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/AdminGetProjectComment',
            params: {
                code: code,
                lastDatetime: lastDatetime
            }
        });

        return request;
    }

    // Function to ger a project by ProjectCode
    this.getUpdateLogList = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/AdminGetUpdateLogList',
            params: {
                code: code
            }
        });

        return request;
    }

    // Function to change project's status
    this.changeProjectStatus = function (project) {
        var request = $http({
            method: 'put',
            url: '/api/ProjectApi/AdminChangeProjectStatus/',
            data: project
        });

        return request;
    }

    // Function to get list backer and backing event
    this.getListBacker = function (code) {
        var request = $http({
            method: 'get',
            url: '/api/ProjectApi/AdminGetListBacker',
            params: {
                code: code
            }
        });
        return request;
    }
});