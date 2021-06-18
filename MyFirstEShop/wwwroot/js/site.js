const hideClassName = "user-hide";

function ReSizeMenu() {
    var p = document.getElementById("Background-SlidBar");
    var x = document.getElementById("SlidBar");


    if (x.style.width == "20%") {
        
        p.style.width = "0";      
        p.style.opacity = 0;
        
        x.style.width = "0";
    }
    else {
        
        x.style.width = "20%";
        
        p.style.width = "100%";

        p.style.opacity = "60%";
        x.style.opacity = "100%";
    }
}


function SettingMenu(x)
{
    panels = document.querySelectorAll(".Group-Panel-Setting > .Panel-Setting");
    panels.forEach(item => {
        if (!item.classList.contains(hideClassName))
            item.classList.add(hideClassName);
    });
    x.classList.remove(hideClassName);
}
