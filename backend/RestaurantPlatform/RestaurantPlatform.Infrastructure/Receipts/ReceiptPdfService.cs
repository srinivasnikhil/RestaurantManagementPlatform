using RestaurantPlatform.Application.DTOs;
using RestaurantPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace RestaurantPlatform.Infrastructure.Receipts
{
    public class ReceiptPdfService : IReceiptService
    {
        public byte[] GenerateReceiptPdf(OrderDto order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text("Dosthi Chowrastha").FontSize(16).Bold();
                        col.Item().AlignCenter().Text("Tampa, FL").FontSize(9).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingVertical(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(4);
                        col.Item().Text($"Order #{order.Id}").Bold();
                        col.Item().Text($"Date: {order.CreatedAt:g}");
                        col.Item().Text($"Type: {order.Type}");

                        if (!string.IsNullOrWhiteSpace(order.CustomerName))
                            col.Item().Text($"Customer: {order.CustomerName}");
                        if (!string.IsNullOrWhiteSpace(order.CustomerPhone) && order.CustomerPhone != "N/A")
                            col.Item().Text($"Phone: {order.CustomerPhone}");
                        if (!string.IsNullOrWhiteSpace(order.CustomerAddress))
                            col.Item().Text($"Address: {order.CustomerAddress}");

                        col.Item().PaddingVertical(6).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);   // qty
                                columns.RelativeColumn();     // name
                                columns.ConstantColumn(70);   // line total
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Qty").Bold();
                                header.Cell().Text("Item").Bold();
                                header.Cell().AlignRight().Text("Total").Bold();
                            });

                            foreach (var item in order.Items)
                            {
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text(item.MenuItemName);
                                table.Cell().AlignRight().Text($"${item.LineTotal:0.00}");
                            }
                        });

                        col.Item().PaddingVertical(6).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Subtotal");
                            row.ConstantItem(70).AlignRight().Text($"${order.Subtotal:0.00}");
                        });
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Tax");
                            row.ConstantItem(70).AlignRight().Text($"${order.Tax:0.00}");
                        });
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Total").Bold();
                            row.ConstantItem(70).AlignRight().Text($"${order.Total:0.00}").Bold();
                        });
                    });

                    page.Footer().Column(col =>
                    {
                        col.Item().PaddingTop(10).AlignCenter().Text("Thank you for your order!").FontSize(9);
                        col.Item().AlignCenter().Text($"Track: {order.TrackingCode}")
                            .FontSize(7).FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
