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

function AddProductMenu()
{
    var panel = document.getElementById("ProductMenu");

    if (panel.classList.contains(hideClassName))
    {
        panel.classList.remove(hideClassName);
    }
    else
    {
        panel.classList.add(hideClassName);
    }


}

function TeacherBtn(x)
{
    document.querySelectorAll(".SettingBtn-Ul > li.SettingBtn").forEach(q => {

        if (q.classList.contains("TeacherBtnStyl")) {
            q.classList.remove("TeacherBtnStyl");
        }

    });

    x.classList.add("TeacherBtnStyl");
}


function ModalCO()
{
    var panel = document.getElementById("Remove-Product");

    panel.classList.toggle(hideClassName);
}

function DeleteProductModal(productID)
{
    ModalCO();

    var input = document.getElementById("RemoveProduct");

    input.value = productID;
    
}

