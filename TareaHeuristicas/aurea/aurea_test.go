package main

import(
	"testing"
)

func TestX1Value(t *testing.T){
	var want float64 = 1.528
	var xmin float64 = 0
	var xmax float64 = 4
	var r float64 = 0.618
	got := xValue(xmax,xmin,r,true)
	if got != want{
		t.Errorf("Result : %f . Wanted : %f",got,want)
	}
}

func TestX2Value(t *testing.T){
	var want float64 = 2.472
	var xmin float64 = 0
	var xmax float64 = 4
	var r float64 = 0.618
	got := xValue(xmax,xmin,r,false)
	if got != want{
		t.Errorf("Result : %f . Wanted : %f",got,want)
	}
}