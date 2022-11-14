# -*- coding: utf-8 -*-
"""
Created on Tue May 24 13:45:06 2022

@author: minji
"""

import random as r
from math import sqrt
from collections import Counter
import numpy as np


def distance_euclidienne(e1, e2):
    somme = 0
    for i in range(len(e1)):
        somme += (float(e1[i]) - float(e2[i]))**2
    return np.sqrt(somme)


class KNN:
    def __init__(self, k=11):
        self.k = k
        
    #X donnes d'entraînement, y les étiquettes associées
    def match(self, X, y):
        self.X_train = X
        self.y_train = y
    
    def predict(self, X):
        yp = [self._predict(x) for x in X]
        return np.array(yp)
        
    def _predict(self, x):
        # calculer la distance
        distance = [distance_euclidienne(x, x_train) for x_train in self.X_train]
        
        # les k plus proches voisins et leurs étiquettes
        k_index = np.argsort(distance)[:self.k]
        k_plus_proches_etiquettes_des_voisins = [self.y_train[i] for i in k_index]
        
        # choisir l'étiquette la plus utilisée dans le voisinage
        
        plus_utilise = Counter(k_plus_proches_etiquettes_des_voisins).most_common(1)
        return plus_utilise[0][0]
        

fichier = open(r"data.txt",'r')

xtrain = []
labelstrain = []
while True:
    l = fichier.readline()
    
    # Si la ligne est vide, la fin du fichier a été atteinte
    if l == "":
        break
    xt = (l.replace("\n","")).split(";")
    donnees = [float(xt[0]),float(xt[1]),float(xt[2]),float(xt[3]),
    float(xt[4]),float(xt[5]),float(xt[6]),float(xt[7]),
    float(xt[8]),float(xt[9])]
    xtrain.append(donnees)
    labelstrain.append(xt[10])


donnees_test = open(r"finalTest.txt",'r')

test = []
#labeltest = []
while True:
    l = donnees_test.readline()
    # Si la ligne est vide, la fin du fichier a été atteinte
    if l == "":
        break
    tst = (l.replace("\n","")).split(";")
    donnees = [float(tst[0]),float(tst[1]),float(tst[2]),float(tst[3]),
    float(tst[4]),float(tst[5]),float(tst[6]),float(tst[7]),
    float(tst[8]),float(tst[9])]
    test.append(donnees)
    #labeltest.append(tst[10])


knn = KNN(15)


knn.match(xtrain, labelstrain)
pred = knn.predict(test)

fichier = open("Pierre_Langhade_Minji_Kim_GroupeO_ClassificationKNN.txt", "w")
for i in pred:
    fichier.write(i)
    fichier.write("\n")
fichier.close()
	


