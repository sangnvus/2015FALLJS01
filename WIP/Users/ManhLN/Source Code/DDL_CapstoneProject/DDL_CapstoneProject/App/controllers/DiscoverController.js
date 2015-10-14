"use strict";

app.controller('DiscoverController', function ($scope, projectstatisticlist, categoryprojectcount) {
    //Todo here.
    $scope.popularproject = projectstatisticlist.data.Data[0];
    $scope.newestproject = projectstatisticlist.data.Data[1];
    $scope.mostfundproject = projectstatisticlist.data.Data[2];
    $scope.enddateproject = projectstatisticlist.data.Data[3];
    //$scope.projectByCategory = popularprojectwithcategory.data.Data;
    $scope.categoryprojectcount = categoryprojectcount.data.Data;
});