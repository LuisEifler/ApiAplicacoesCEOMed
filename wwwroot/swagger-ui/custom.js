document.addEventListener('DOMContentLoaded', () => {
    const element = document.createElement('div');
    element.classList.add('painelLateral');
    element.innerHTML = `
        <h3>Painel Lateral</h3>
        <p>Conteúdo personalizado do Swagger.</p>
    `;

    //document.body.appendChild(element);

    setTimeout(() => {
        let logo = document.createElement('img');
        document.getElementsByClassName('topbar-wrapper')[0].appendChild(logo)
        document.querySelectorAll('a[class="link"]').forEach(el => el.remove());
        document.querySelectorAll('form[class="download-url-wrapper"]').forEach(el => el.remove());
    }, 1500)

    document.querySelectorAll('link[rel="icon"]').forEach(el => el.remove());
   
    const link = document.createElement('link');
    link.rel = 'icon';
    link.type = 'image/png';
    link.href = '../swagger-ui/ceomedLogoColorida.png?v=' + Date.now(); // força recarregamento
    document.head.appendChild(link);

});

const style = document.createElement('style');
style.innerHTML = `
//.painelLateral {
//    position: fixed;
//    top: 0;
//    left: 0;
//    width: 200px;
//    height: 100%;
//    background: #537aef ;
//    padding: 20px;
//    z-index: 9999;
//    overflow-y: auto;
//}

.img{
    position:absolute;
    top:0;
    left:400;
}

//#swagger-ui{
//    position: absolute;
//    top: 0;
//    left: 200px;
//    width:calc(100% - 200px);
//}
`;

//document.head.appendChild(style);

//(function () {
//    window.addEventListener("load", function () {
//        setTimeout(function () {

//            var logo = document.getElementsByClassName('link');

//            if (logo.length > 0 && logo[0].children.length > 0) {
//                var imgElement = logo[0].children[0];
//                imgElement.alt = "Logo";
//                imgElement.src = "/swagger-ui/ceomed_LogoPreta.png";
//                imgElement.style.height = "80px"; // Set height to 80px
//            }
//        }, 0); // You can adjust the timeout if needed
//    });
//})();
