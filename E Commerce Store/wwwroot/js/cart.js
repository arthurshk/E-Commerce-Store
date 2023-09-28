(function ($) {
    "use strict";
    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        button.closest("div.quantity-wrapper").find('input').val(newVal).change();
        button.closest("div.quantity-wrapper").find("button.add-cart").attr("data-quantity", newVal)

    });

    $("button.add-cart").on('click', e => {
        let button = $(e.target)
        let productId = button.data('id')
        let quantity = button.data('quantity')

        console.log("ADD TO CART", {
            productId: productId,
            quantity: quantity
        })

        fetch("/api/add-to-cart", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                productId: productId,
                quantity: quantity
            })
        }).then(r => r.json())
            .then(data => {
                console.log(data)
                $("#cart-product-count").text(data.cartProductsCount)

            })
    })

})(jQuery);

