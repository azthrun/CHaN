import 'package:cloud_firestore/cloud_firestore.dart';
import 'package:inventory/abstractions.dart';

class Inventory implements GenericModel {
  @override
  String? id;
  @override
  Timestamp? updatedAt;

  String? name;
  String? description;
  String? stockLocation;
  int? quantity;
  double? buyInPrice;
  int? historySold;
  String? imagePath;

  Inventory({
    this.id,
    required this.name,
    this.description,
    this.stockLocation,
    this.quantity = 0,
    this.buyInPrice,
    this.historySold = 0,
    this.imagePath,
    required this.updatedAt,
  });

  factory Inventory.fromJson(Map<String, dynamic> json) {
    return Inventory(
      id: json['id'],
      name: json['name'],
      description: json['description'],
      stockLocation: json['stockLocation'],
      quantity: json['quantity'] as int?,
      buyInPrice: json['buyInPrice'] as double?,
      historySold: json['historySold'] as int?,
      imagePath: json['imagePath'],
      updatedAt: json['updatedAt'] as Timestamp?,
    );
  }

  @override
  Map<String, dynamic> toJson({bool keepUpdatedAt = true}) {
    final map = {
      'id': id,
      'name': name,
      'description': description,
      'stockLocation': stockLocation,
      'quantity': quantity,
      'buyInPrice': buyInPrice,
      'historySold': historySold,
      'imagePath': imagePath,
      'updatedAt': updatedAt,
    };
    map.removeWhere((key, value) =>
        value == null || (!keepUpdatedAt && key == 'updatedAt'));
    return map;
  }
}
