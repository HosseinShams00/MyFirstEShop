
function ReSizeMenu() {
    var p = document.getElementById("Background-SlidBar");
    var x = document.getElementById("SlidBar");
    var body = document.getElementsByTagName("main")[0];


    if (x.style.width == "20%") {
        
        p.style.width = "0";      
        p.style.opacity = 0;
        
        x.style.width = "0";
        body.style.marginRight = "0";
    }
    else {
        
        x.style.width = "20%";
        
        p.style.width = "100%";
        //p.style.backgroundColor = "rgba(0 , 0 , 0 , 0.8)";

        p.style.opacity = "60%";
        x.style.opacity = "100%";
        body.style.marginRight = "20%";
    }
}

function ErrorSpn() {


    var x = document.querySelectorAll(".field-validation-error");

    //var x = document.getElementsByClassName("field-validation-error");
    x.forEach((item) => {

        item.addEventListener("change", () => {

            if (item.textContent != null && !item.classList.contains("Error-Style")) {
                item.classList.add("Error-Style");
            }
            else {
                item.classList.remove("Error-Style");
            }

        })
    });

    alert(x.length);
    /*    var p = document.getElementById("test").classList.add("Error-Style");*/

}
