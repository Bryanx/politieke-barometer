$(document).ready(function () {
    $(function () {
        $('.grid-stack').gridstack();
    });

    $(function () {
        var options = {
            float: true
        };
        $('.grid-stack').gridstack(options);

        new function () {
            this.items = [
                {x: 0, y: 0, width: 2, height: 2},
                {x: 3, y: 1, width: 1, height: 2},
                {x: 4, y: 1, width: 1, height: 1},
                {x: 2, y: 3, width: 3, height: 1},
//                    {x: 1, y: 4, width: 1, height: 1},
//                    {x: 1, y: 3, width: 1, height: 1},
//                    {x: 2, y: 4, width: 1, height: 1},
                {x: 2, y: 5, width: 1, height: 1}
            ];

            this.grid = $('.grid-stack').data('gridstack');

            this.addNewWidget = function () {
                var node = this.items.pop() || {
                    x: 12 * Math.random(),
                    y: 5 * Math.random(),
                    width: 1 + 3 * Math.random(),
                    height: 1 + 3 * Math.random()
                };
                this.grid.addWidget($('<div><div class="grid-stack-item-content" /><div/>'),
                    node.x, node.y, node.width, node.height);
                return false;
            }.bind(this);

            $('#add-new-widget').click(this.addNewWidget);
        };
    });

});