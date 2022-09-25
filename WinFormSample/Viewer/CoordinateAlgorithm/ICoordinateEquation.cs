﻿/*
 *@title WinFormGUI / WinFormSample / Viewer / CoordinateAlgorithm
 *@class interface ICoordinateEquation
 *@class   └ EquationLinear : ICoordinateEquation
 *@class   └ EquationQuadratic : ICoordinateEquation
 *@content 式の係数値を保持するクラス
 *         ・ICoordinateEquationで同一視
 *         ・Algo系クラス継承階層に同名メソッドを用意し、
 *           ポリモーフィズムによって呼出メソッドを切り替えることができるようにする
 *           (クラスの if分岐ではなく、対象オブジェクトの型、引数型によって振り分け)
 *           
 *@subject use
 *         AlgoXxxx.AlgoFuncionXtoY(float x, ICoordinate)
 *         AlgoXxxx.AlgoFuncionYtoX(float y, ICoordinate)
 *         AlgoXxxx.AlgoIntercrptX(ICoordinateEquation)
 *         AlgoXxxx.AlgoIntercrptY(ICoordinateEquation)
 *         AlgoXxxx.DrawMultiQuadraticFunction(ICoordinate[])
 */

namespace WinFormGUI.WinFormSample.Viewer.CoordinateAlgorithm
{
    interface ICoordinateEquation
    {
        (decimal a, decimal b, decimal c) GetGeneralParam();
        
        string ToString();
    }
}
