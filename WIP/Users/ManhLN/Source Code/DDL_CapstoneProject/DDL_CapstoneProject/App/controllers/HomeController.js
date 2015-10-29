"use strict";

app.controller('HomeController', function ($scope, slides, liststatisticforhome) {
    //Todo here.
    $scope.Slides = slides.data.Data;

    var data = liststatisticforhome.data.Data;
    $scope.popularproject = data.popularproject;
    $scope.projectByCategory = data.popularproject;
    $scope.highestprojectpledge = data.highestprojectpledge[0];
    $scope.highestprojectfund = data.highestprojectfund[0];
    $scope.totalprojectfund = data.totalprojectfund[0];

});