$(document).ready(function () {
    $(function () {
        $('.grid-stack').gridstack();
    });


    ko.components.register('dashboard-grid', {
        viewModel: {
            createViewModel: function (controller, componentInfo) {
                var ViewModel = function (controller, componentInfo) {
                    var grid = null;

                    this.widgets = controller.widgets;

                    this.afterAddWidget = function (items) {
                        if (grid == null) {
                            grid = $(componentInfo.element).find('.grid-stack').gridstack({
                                auto: false,
                                width: 4,
                            }).data('gridstack');
                        }

                        var item = _.find(items, function (i) { return i.nodeType == 1 });
                        grid.addWidget(item);
                        ko.utils.domNodeDisposal.addDisposeCallback(item, function () {
                            grid.removeWidget(item);
                        });
                    };
                };

                return new ViewModel(controller, componentInfo);
            }
        },
        template: { element: 'gridstack-template' }
    });

    $(function () {
        var Controller = function (widgets) {
            var self = this;

            this.widgets = ko.observableArray(widgets);

            this.addNewWidget = function () {
                this.widgets.push({
                    x: 0,
                    y: 0,
                    width: Math.floor(1 + 4 * Math.random()),
                    height: Math.floor(1 + 4 * Math.random()),
                    auto_position: true
                });
                return false;
            };

            this.deleteWidget = function (item) {
                self.widgets.remove(item);
                return false;
            };
        };

        var widgets = [
            {x: 0, y: 0, width: 3, height: 2},
            {x: 4, y: 0, width: 4, height: 2},
            {x: 6, y: 0, width: 3, height: 4},
            {x: 1, y: 2, width: 4, height: 2}
        ];

        var controller = new Controller(widgets);
        ko.applyBindings(controller);
    });


});