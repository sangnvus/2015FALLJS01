"use strict";

app.controller('HomeController', function ($scope, SlideService) {
    //Todo here.
    loadAllSlidesRecords();
    function loadAllSlidesRecords() {
        var promiseGetSlide = SlideService.getSlides();

        promiseGetSlide.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Slides = result.data.Data;
                } else if (result.data.Status === "error") {
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }
});