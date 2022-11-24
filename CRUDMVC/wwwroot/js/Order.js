document.getElementById("AddOrderItemBtn").addEventListener("click", function () {
    var elements = document.getElementsByClassName("name")
    var count = elements.length

    var newNameLabel = document.createElement("label")
    var newQuantityLabel = document.createElement("label")
    var newUnitLabel = document.createElement("label")
    /*var newValidation = document.createElement("SPAN")*/

    var newNameInput = document.createElement("input")
    var newQuantityInput = document.createElement("input")
    var newUnitInput = document.createElement("input")

    newNameLabel.setAttribute("for", "OrderItems_" + count + "__Name")
    newQuantityLabel.setAttribute("for", "OrderItems_" + count + "__Quantity")
    newUnitLabel.setAttribute("for", "OrderItems_" + count + "__Unit")

    /* < span class="text-danger field-validation-valid" data-valmsg-for= "orderItems[0].Name" data-valmsg-replace= "true" ></span >*/
    //newValidation.setAttribute("data-valmsg-for", "OrderItems[" + count + "].Name")
    //newValidation.setAttribute("data-valmsg-replace", "true")
    //newValidation.setAttribute("class", "text-danger field-validation-valid")
    //newValidation.innerHTML = "ValName"

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

    document.getElementById("orderItem").append(newNameLabel)
    document.getElementById("orderItem").append(newNameInput)
    /*document.getElementById("orderitem").append(newValidation)*/

    document.getElementById("orderItem").append(newQuantityLabel)
    document.getElementById("orderItem").append(newQuantityInput)

    document.getElementById("orderItem").append(newUnitLabel)
    document.getElementById("orderItem").append(newUnitInput)
})