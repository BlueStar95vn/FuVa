var serviceBase = 'http://localhost:63237/';
angular.
    module('vacationApp').
    config(['$locationProvider', '$routeProvider',
        function config($locationProvider, $routeProvider) {
            $locationProvider.hashPrefix("");
            $routeProvider.
                when('/login', {
                    template: '<login></login>'
                }).
                when('/employees', {
                    template: '<employee-list></employee-list>'
                }).
                when('/employees/detail', {
                    template: '<employee-detail></employee-detail>'
                }).
                when('/employees/edit', {
                    template: '<employee-edit></employee-edit>'
                }).
                when('/employees/add', {
                    template: '<employee-add></employee-add>'
                }).
                when('/teams', {
                    template: '<team-list></team-list>'
                }).
                when('/teams/detail', {
                    template: '<team-detail></team-detail>'
                }).
                when('/teams/edit', {
                    template: '<team-edit><team-edit>'
                }).
                when('/teams/add', {
                    template: '<team-add></team-add>'
                }).
                when('/vacation', {
                    template: '<vacation></vacation>'
                }).
                when('/report', {
                    template: '<report></report>'
                }).
                otherwise({
                    redirectTo: '/login'
                });
        }
    ]).
    run(function ($rootScope, $location) {
        $rootScope.location = $location;

    });