angular.
    module('vacationApp').
    config(['$locationProvider', '$routeProvider',
        function config($locationProvider, $routeProvider) {
            $locationProvider.hashPrefix("");
            $routeProvider.
                when('/login', {
                    template: '<login></login>',                    
                    restrictions: {
                        ensureAuthenticated: false,
                        loginRedirect: false
                    }                 
                }).
                when('/logout', {
                    template: '',
                    controller: 'authLogoutController',
                    controllerAs:'logoutCtrl',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/status', {
                    templateUrl: 'app/Auth/auth.status.view.html',
                    controller: 'authStatusController',
                    controllerAs: 'authStatusCtrl',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees', {
                    template: '<employee-list></employee-list>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees/detail', {
                    template: '<employee-detail></employee-detail>',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees/edit', {
                    template: '<employee-edit></employee-edit>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/employees/add', {
                    template: '<employee-add></employee-add>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams', {
                    template: '<team-list></team-list>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams/detail', {
                    template: '<team-detail></team-detail>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams/edit', {
                    template: '<team-edit><team-edit>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/teams/add', {
                    template: '<team-add></team-add>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/vacation', {
                    template: '<vacation></vacation>',
                     restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                when('/report', {
                    template: '<report></report>',
                     restrictions: {
                         ensureAuthenticated: true,
                        loginRedirect: false
                    }
                }).
                otherwise({
                    redirectTo: '/login',
                    restrictions: {
                        ensureAuthenticated: true,
                        loginRedirect: false
                    }
                });
        }
    ]).
    run(function routeStart($rootScope, $location, $route) {       
        $rootScope.location = $location;
        $rootScope.$on('$routeChangeStart', (event, next, current) => {
            if (next.restrictions.ensureAuthenticated) {
                if (!localStorage.getItem('token')) {
                    $location.path('/login');
                }
            }
            if (next.restrictions.loginRedirect) {
                if (localStorage.getItem('token')) {                   
                    $location.path('/status');
                }
            }
        })
    });