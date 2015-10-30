"use strict";

service.service("AdminSlideService", function ($http) {
    //Read all Slides for administrator.
    this.getSlides = function () {
        return $http.get("../api/SlideApi/GetSlidesForAdmin");
    };
    
    // Function to edit a Project
    this.addSlide = function (slide, file) {
        var fdata = new FormData();
        var url = "../api/SlideApi/CreateSlide";

        fdata.append('file', file);
        fdata.append('slide', JSON.stringify(slide));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    }

    // delete slide
    this.deleteSlide = function (id) {
        var request = $http({
            method: 'delete',
            url: '../api/SlideApi/DeleteSlide',
            params: {
                id: id
            }
        });
        return request;
    }


    // Change status of category
    this.changeSlideStatus = function (id) {
        var request = $http({
            method: 'put',
            url: '../api/SlideApi/ChangeSlideStatus',
            params: {
                id: id
            }
        });
        return request;
    }

    // Function to edit a Project
    this.editSlide = function (slide, file) {
        var fdata = new FormData();
        var url = "../api/SlideApi/EditSlide";

        fdata.append('file', file);
        fdata.append('slide', JSON.stringify(slide));

        return $http.post(url, fdata, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        });
    }

    // Function to edit a Project
    this.changeOrder = function (id, type) {
        var request = $http({
            method: 'put',
            url: '../api/SlideApi/ChangeOrder',
            params: {
                id: id,
                type: type
            }
        });
        return request;
    }
});