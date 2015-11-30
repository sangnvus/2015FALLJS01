"use strict";

app.controller('HomeController', function ($scope, $sce, $window, slides, liststatisticforhome) {
    //Todo here.
    $scope.Slides = slides.data.Data;

    var data = liststatisticforhome.data.Data;
    $scope.popularproject = data.popularproject;
    $scope.projectByCategory = data.projectByCategory;
    $scope.highestprojectpledge = data.highestprojectpledge[0];
    $scope.highestprojectfund = data.highestprojectfund[0];
    $scope.totalprojectfund = data.totalprojectfund[0];
    if (typeof ($scope.highestprojectfund) == "undefined") {
        $scope.highestprojectfund = { 'CurrentFundedNumber': 0 };
    }
    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    $scope.resizeWindow = function () {
        $(window).triggerHandler('resize');
    };
});