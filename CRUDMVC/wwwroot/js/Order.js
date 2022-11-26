document.getElementById("AddOrderItemBtn").addEventListener("click", function () {
    var elements = document.getElementsByClassName("name")
    var count = elements.length

    var newNameLabel = document.createElement("label")
    var newQuantityLabel = document.createElement("label")
    var newUnitLabel = document.createElement("label")

    var newNameInput = document.createElement("input")
    var newQuantityInput = document.createElement("input")
    var newUnitInput = document.createElement("input")

    
        var newInputItemId = document.createElement("input")
        var newInputOrderId = document.createElement("input")
        newInputItemId.setAttribute("id", "OrderItems_" + count + "__Id")
        newInputOrderId.setAttribute("id", "OrderItems_" + count + "__OrderId")
        newInputItemId.setAttribute("data-val", "true")
        newInputItemId.setAttribute("data-val-required", "The Id field is required.")
        newInputItemId.setAttribute("name", "OrderItems[" + count + "].Id")
        newInputOrderId.setAttribute("name", "OrderItems[" + count + "].OrderId")
        newInputItemId.setAttribute("type", "hidden")
        newInputOrderId.setAttribute("type", "hidden")
    


    newNameLabel.setAttribute("for", "OrderItems_" + count + "__Name")
    newQuantityLabel.setAttribute("for", "OrderItems_" + count + "__Quantity")
    newUnitLabel.setAttribute("for", "OrderItems_" + count + "__Unit")

    

    newNameLabel.innerHTML = "Name"
    newQuantityLabel.innerHTML = "Quantity"
    newUnitLabel.innerHTML = "Unit"

    newNameLabel.setAttribute("class", "control-label")
    newQuantityLabel.setAttribute("class", "control-label")
    newUnitLabel.setAttribute("class", "control-label")

    newNameInput.setAttribute("id", "OrderItems_" + count + "__Name")
    newQuantityInput.setAttribute("id", "OrderItems_" + count + "__Quantity")
    newUnitInput.setAttribute("id", "OrderItems_" + count + "__Unit")

    newNameInput.setAttribute("class", "form-control name")
    newQuantityInput.setAttribute("class", "form-control quantity")
    newUnitInput.setAttribute("class", "form-control unit")

    newNameInput.setAttribute("name", "OrderItems[" + count + "].Name")
    newQuantityInput.setAttribute("name", "OrderItems[" + count + "].Quantity")
    newUnitInput.setAttribute("name", "OrderItems[" + count + "].Unit")

    newNameInput.setAttribute("type", "text")
    newQuantityInput.setAttribute("type", "decimal")
    newUnitInput.setAttribute("type", "text")

    
        document.getElementById("orderItem").append(newInputItemId)
        document.getElementById("orderItem").append(newInputOrderId)
    

    document.getElementById("orderItem").append(newNameLabel)
    document.getElementById("orderItem").append(newNameInput)
    /*document.getElementById("orderitem").append(newValidation)*/

    document.getElementById("orderItem").append(newQuantityLabel)
    document.getElementById("orderItem").append(newQuantityInput)

    document.getElementById("orderItem").append(newUnitLabel)
    document.getElementById("orderItem").append(newUnitInput)
})

function AddOrderItemBtn(isEditView) {
    var elements = document.getElementsByClassName("name")
    var count = elements.length

    var newNameLabel = document.createElement("label")
    var newQuantityLabel = document.createElement("label")
    var newUnitLabel = document.createElement("label")

    var newNameInput = document.createElement("input")
    var newQuantityInput = document.createElement("input")
    var newUnitInput = document.createElement("input")

    if (isEditView) {
        var newInputItemId = document.createElement("input")
        var newInputOrderId = document.createElement("input")
        newInputItemId.setAttribute("id", "OrderItems_" + count + "__Id")
        newInputOrderId.setAttribute("id", "OrderItems_" + count + "__OrderId")
        newInputItemId.setAttribute("data-val", "true")
        newInputItemId.setAttribute("data-val-required", "The Id field is required.")
        newInputItemId.setAttribute("name", "OrderItems[" + count + "].Id")
        newInputOrderId.setAttribute("name", "OrderItems[" + count + "].OrderId")
        newInputItemId.setAttribute("type", "hidden")
        newInputOrderId.setAttribute("type", "hidden")
    }


    newNameLabel.setAttribute("for", "OrderItems_" + count + "__Name")
    newQuantityLabel.setAttribute("for", "OrderItems_" + count + "__Quantity")
    newUnitLabel.setAttribute("for", "OrderItems_" + count + "__Unit")



    newNameLabel.innerHTML = "Name"
    newQuantityLabel.innerHTML = "Quantity"
    newUnitLabel.innerHTML = "Unit"

    newNameLabel.setAttribute("class", "control-label")
    newQuantityLabel.setAttribute("class", "control-label")
    newUnitLabel.setAttribute("class", "control-label")

    newNameInput.setAttribute("id", "OrderItems_" + count + "__Name")
    newQuantityInput.setAttribute("id", "OrderItems_" + count + "__Quantity")
    newUnitInput.setAttribute("id", "OrderItems_" + count + "__Unit")

    newNameInput.setAttribute("class", "form-control name")
    newQuantityInput.setAttribute("class", "form-control quantity")
    newUnitInput.setAttribute("class", "form-control unit")

    newNameInput.setAttribute("name", "OrderItems[" + count + "].Name")
    newQuantityInput.setAttribute("name", "OrderItems[" + count + "].Quantity")
    newUnitInput.setAttribute("name", "OrderItems[" + count + "].Unit")

    newNameInput.setAttribute("type", "text")
    newQuantityInput.setAttribute("type", "decimal")
    newUnitInput.setAttribute("type", "text")

    if (isEditView) {
        document.getElementById("orderItem").append(newInputItemId)
        document.getElementById("orderItem").append(newInputOrderId)
    }

    document.getElementById("orderItem").append(newNameLabel)
    document.getElementById("orderItem").append(newNameInput)
    /*document.getElementById("orderitem").append(newValidation)*/

    document.getElementById("orderItem").append(newQuantityLabel)
    document.getElementById("orderItem").append(newQuantityInput)

    document.getElementById("orderItem").append(newUnitLabel)
    document.getElementById("orderItem").append(newUnitInput)
}