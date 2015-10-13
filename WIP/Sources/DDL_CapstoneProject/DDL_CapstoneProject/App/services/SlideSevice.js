service.service("SlideService", function ($http) {

    //Read all Students  
    this.getSlides = function () {
        return $http.get("api/SlideAPI/GetSlides");
    };
});