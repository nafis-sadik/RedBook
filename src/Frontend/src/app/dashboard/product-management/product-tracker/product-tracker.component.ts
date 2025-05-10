import { Component, Input } from '@angular/core';
import { PurchaseInvoiceDetailsModel } from '../../Models/purchase-invoice-details.model';
import JsBarcode from 'jsbarcode';

@Component({
  selector: 'app-product-tracker',
  templateUrl: './product-tracker.component.html',
  styleUrls: ['./product-tracker.component.scss']
})
export class ProductTrackerComponent {
  @Input() invoiceDetailsModel: PurchaseInvoiceDetailsModel;
  @Input() invoiceNumber: string;
  @Input() outletName: string;
  
  ngAfterViewInit () {
    if(this.invoiceDetailsModel && this.invoiceNumber && this.outletName && this.outletName) {      
      JsBarcode('#barcode', this.invoiceDetailsModel.barCode, {
        format: 'CODE128',
        lineColor: '#000',
        background: '#fff',
        width: 1,
        height: 80,
        displayValue: false,
      });
    }
  }
  
  printInvoice() {
    alert("Print Invoice Featire shall be available soon!");
    return;
  }

  // printInvoice() {
  //   // Get the content of the div
  //   let content = document.getElementById("Tracker");
  
  //   // Get the canvas and its context
  //   let canvas = document.createElement("canvas");
  //   let context = canvas.getContext("2d");
  //   if (!content || !canvas || !context) {
  //     console.error('Element not found!');
  //     return;
  //   }
  
  //   // Set the canvas size to match the div's dimensions
  //   const width = content.offsetWidth;
  //   const height = content.offsetHeight;
  //   canvas.width = width;
  //   canvas.height = height;
  
  //   // Set the background color
  //   const style = getComputedStyle(content);
  //   context.fillStyle = style.backgroundColor || "white";
  //   context.fillRect(0, 0, width, height);
  
  //   // Draw text content onto the canvas
  //   context.font = "16px Arial";
  //   context.fillStyle = style.color || "black";
  //   let yPosition = 20;

  //   // Loop through child nodes and only process elements
  //   let childNodes = Array.from(content.childNodes);
  //   for(let i = 0; i < childNodes.length; i++) {
  //     let node = childNodes[i];
  //     if (node.nodeType === Node.ELEMENT_NODE) {
  //       const element = node as HTMLElement; // Cast to HTMLElement
  //       context.fillText(element.textContent?.trim() || '', 10, yPosition);
  //       yPosition += 20;
  //     } else if (node.nodeType === Node.TEXT_NODE) {
  //       context.fillText(node.textContent?.trim() || '', 10, yPosition);
  //       yPosition += 20;
  //     }
  //   }
  
  //   // Convert the canvas to an image
  //   const imageData = canvas.toDataURL("image/png");
  
  //   // Open a new window, display the image, and print it
  //   const newWindow = window.open("", "_blank");
  //   if (newWindow) {
  //     newWindow.document.write(`
  //       <html>
  //         <head>
  //           <title>Print</title>
  //         </head>
  //         <body style="margin: 0;">
  //           <img src="${imageData}" style="width: 100%; display: block;" />
  //           <script>
  //             window.onload = function() {
  //               window.print();
  //               window.close();
  //             };
  //           </script>
  //         </body>
  //       </html>
  //     `);
  //     // newWindow.document.close();
  //   }
  // }
}
