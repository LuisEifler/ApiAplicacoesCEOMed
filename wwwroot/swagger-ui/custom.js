document.addEventListener('DOMContentLoaded', () => {
    const element = document.createElement('div');
    element.classList.add('painelLateral');
    element.innerHTML = `
        <h3>Painel Lateral</h3>
        <p>Conteúdo personalizado do Swagger.</p>
    `;
    document.body.appendChild(element);
});

const style = document.createElement('style');
style.innerHTML = `
.painelLateral {
    position: fixed;
    top: 0;
    left: 0;
    width: 200px;
    height: 100%;
    background: #537aef ;
    padding: 20px;
    z-index: 9999;
    overflow-y: auto;
}

#swagger-ui{
    position: absolute;
    top: 0;
    left: 200px;
    width:calc(100% - 200px);
}
`;
document.head.appendChild(style);
