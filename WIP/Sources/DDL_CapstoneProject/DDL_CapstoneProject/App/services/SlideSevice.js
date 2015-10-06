service.service("SlideService", function ($http) {

    //Read all Students  
    this.getSlides = function () {
        return $http.get("api/SlideAPI/GetSlides");
    };

    //Fundction to Read Student by Student ID  
    this.getSlide = function (id) {
        return $http.get("api/SlidesAPI/" + id);
    };

    //Function to create new Student  
    this.post = function (Student) {
        var request = $http({
            method: "post",
            url: "api/SlidesAPI",
            data: Student
        });
        return request;
    };

    //Edit Student By ID   
    this.put = function (id, Student) {
        var request = $http({
            method: "put",
            url: "/api/SlidesAPI/" + id,
            data: Student
        });
        return request;
    };

    //Delete Student By Student ID  
    this.delete = function (id) {
        var request = $http({
            method: "delete",
            url: "api/SlidesAPI/" + id
        });
        return request;
    };
});