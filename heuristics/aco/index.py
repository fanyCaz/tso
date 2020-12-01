#backtracking
def find_path(graph, start,end,path=[]):
    path.append(start)
    if start == end:
        return path
    if start not in graph:
        return None
    paths = []
    for node in graph[start]:
        if node not in path:
            newPath = find_path(graph,node,end,path)
            for new in newPath:
                paths.append(new)
            if newPath : return newPath
    return None

visitados = []
def vecinoProximo(graph,inicio,fin, camino=[]):
    if inicio == fin:
        print("si")
        return camino
    at = inicio
    visitados.append(at)
    costoMinimo = 100**100 #valor muy grande
    vecinos = graph[at]
    #camino.append(inicio)
    posibleCamino = inicio

    for i in vecinos:
        print("nodo {} costo {} ".format(i,vecinos[i]))
        if i not in visitados:
            if vecinos[i] < costoMinimo:
                costoMinimo = vecinos[i]
                posibleCamino = i
                print("menor: {}".format(i) )
    camino.append(posibleCamino)
    #print(camino)
    #print(visitados)
    inicio = posibleCamino
    #print(inicio)
    vecinoProximo(graph,inicio,fin,camino)

    

def main():
    graph = {'A' : {'B' : 14,'C' : 16, 'D': 14, 'E': 17},
         'B' : {'A' : 14,'C' : 16, 'D': 18, 'E': 15},
         'C' : {'A' : 16,'B' : 16, 'D': 17, 'E': 16},
         'D' : {'A' : 15,'B' : 18, 'C': 17, 'E': 15},
         'E' : {'A' : 17,'B' : 15, 'C': 16, 'D': 15} }
    #print(graph)
    res = vecinoProximo(graph,'A','D')
    #res = find_path(graph,'A','D')
    print(res)

if __name__ == '__main__':
    main()