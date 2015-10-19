"use strict";

app.controller('HomeController', function ($scope, slides, liststatisticforhome) {
    //Todo here.
    $scope.Slides = slides.data.Data;
    $scope.popularproject = liststatisticforhome.data.Data[0];
    $scope.projectByCategory = liststatisticforhome.data.Data[1];
    $scope.highestprojectpledge = liststatisticforhome.data.Data[2][0];
    $scope.highestprojectfund = liststatisticforhome.data.Data[3][0];
    $scope.totalprojectfund = liststatisticforhome.data.Data[3][1];
});