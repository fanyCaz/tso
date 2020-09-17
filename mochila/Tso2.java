/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package tso2;

/**
 *
 * @author usuario
 */
public class Tso2 {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        
 
        Elemento[] elementos = {
            new Elemento(15, 40),
            new Elemento(12, 28),
            new Elemento(10, 22),
            new Elemento(13, 30),
            new Elemento(22, 54),
            new Elemento(16, 38),
            new Elemento(10, 24),
            new Elemento(14, 35)
                                
        };

        Mochila m_base = new Mochila(60, elementos.length);
        Mochila m_opt = new Mochila(60, elementos.length);

        llenarMochila(m_base, elementos, false, m_opt);

        System.out.println(m_opt);
        
    }
    
    
    public static void llenarMochila(Mochila m_base, Elemento[] elementos, boolean llena, Mochila m_opt) {

        //si esta llena
        if (llena) {
            //compruebo si tiene mas beneficio que otra
            if (m_base.getBeneficio() > m_opt.getBeneficio()) {

                Elemento[] elementosMochBase = m_base.getElementos();
                m_opt.clear();

                //metemos los elementos
                for (Elemento e : elementosMochBase) {
                    if (e != null) {
                        m_opt.aniadirElemento(e);
                    }

                }

            }

        } else {
            //Recorre los elementos
            for (int i = 0; i < elementos.length; i++) {
                //si existe el elemento
                if (!m_base.existeElemento(elementos[i])) {
                    //Si el peso de la mochila se supera, indicamos que esta llena
                    if (m_base.getPesoMaximo() >= m_base.getPeso() + elementos[i].getPeso()) {
                        
                        
                        m_base.aniadirElemento(elementos[i]); //añadimos
                        llenarMochila(m_base, elementos, false, m_opt);
                        m_base.eliminarElemento(elementos[i]); // lo eliminamos
                    } else {
                        llenarMochila(m_base, elementos, true, m_opt);
                    }

                }

            }

        }

    }
    
    
}
