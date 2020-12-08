function llenarMochila(pesoMaximo=600, pesos=[15,12,10,13,22,16,10,14], valores=[40,28,22,30,54,38,24,35]){
    let n = pesos.length;
    numIter += 1 + 3*n + n * Math.log2(n);
    let fracciones = valores.map(v => 0);
    let precios = valores.map((v,i) => [i, v/pesos[i]]).
                  sort((a, b) => b[1]-a[1]).map(v => v[0]);
    let pesoMochila = 0;
    for (let j=0; j<precios.length; j++){
        numIter++;
        let i = precios[j];
        if (pesoMochila+pesos[i]<=pesoMaximo){
            fracciones[i] = 1;
            pesoMochila += pesos[i];
        } else {
            fracciones[i] = (pesoMaximo - pesoMochila) / pesos[i];
            pesoMochila = pesoMaximo;
        }
        if (pesoMochila===pesoMaximo) break;
    }
    return fracciones;
}