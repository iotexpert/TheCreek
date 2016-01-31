/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"

module non_ov_2 (
	input wire clk,
	input wire ref,
    output wire ph1,
	output wire ph2
);

    wire refb = ~ref;
    reg ref1;
    reg ref2;
    reg refb1;
    reg refb2;

    always @(posedge clk)
    begin
        ref1 = ref;
    end

    always @(posedge clk)
    begin
        ref2 = ref1;
    end

    always @(posedge clk)
    begin
        refb1 = refb;
    end

    always @(posedge clk)
    begin
        refb2 = refb1;
    end

    assign ph1 = ref2 & ref;
    assign ph2 = refb2 & refb;

endmodule

